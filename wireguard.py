import requests
import json
import logging
import argparse

import os

# Setup basic logging
logging.basicConfig(level=logging.INFO, format='%(asctime)s - 📜 %(message)s')

# File path to the last known IPs JSON and Telegram Bot details
last_ips_file = 'last_ips.json'
userPrivKey = ""
bot_token = 'x'
chat_id = '1289941194'
# Headers for the WireGuard configuration request
headers = {
    "Host": "my.surfincn.com",
    "Cookie": "",
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:123.0) Gecko/20100101 Firefox/123.0",
    "Accept": "*/*",
    "Accept-Language": "en-US,en;q=0.5",
    "Accept-Encoding": "gzip, deflate, br",
    "Referer": "https://my.surfincn.com/vpn/manual-setup/main/wireguard/generate-key?country=CN&referrer=%2Fvpn%2Fmanual-setup%2Fmain%2Fwireguard%2Fgenerate-key&restricted=",
    "Content-Type": "application/json",
    "Origin": "https://my.surfincn.com",
    "Sec-Fetch-Dest": "empty",
    "Sec-Fetch-Mode": "cors",
    "Sec-Fetch-Site": "same-origin",
    "Te": "trailers",
    "Connection": "close"
}

# Load changed IPs

def load_changed_ips():
    with open(last_ips_file, 'r') as file:
        last_ips = json.load(file)
    return [details for location, details in last_ips.items() if details.get('status') == "✅ Changed"]



# Generate WireGuard config and send to Telegram 
def generate_and_send_wireguard_config(changed_ips, send_as_code):
    for ip_details in changed_ips:
        payload = {
            "pubKey": ip_details['pubKey'],
            "userPrivKey": userPrivKey,
            "connectionName": ip_details['connectionName'],
            "ip": ip_details['ip']
        }
        response = requests.post("https://my.surfincn.com/vpn/wg-config", headers=headers, json=payload)
        if response.status_code == 200:
            config_text = response.text  # Moved inside the if condition
            intro_message = f"🚀🤖 New WireGuard Configuration for \n <b>{ip_details['connectionName'].upper()}</b> "
            send_telegram_message(intro_message, parse_mode="HTML")

            if send_as_code:
                # Send the code block as a separate message
                code_message = f"<pre>{config_text}</pre>"
                send_telegram_message(code_message, parse_mode="HTML")
            else:
                config_filename = f"{ip_details['connectionName']}.conf"
                with open(config_filename, 'w') as file:
                    file.write(config_text)
                logging.info(f"🔐 Saved WireGuard config for: {ip_details['connectionName']}")
                send_telegram_document(config_filename, ip_details['connectionName'])
                os.remove(config_filename)
        else:
            logging.error(f"❌ Failed to generate WireGuard config for {ip_details['connectionName']}")




# Send document to Telegram with custom message
def send_telegram_document(document_path, connection_name):
    url = f"https://api.telegram.org/bot{bot_token}/sendDocument"
    message = f"🚀 New WireGuard Configuration for {connection_name} 🚀\nCheck out the attached configuration file! 📁"
    with open(document_path, 'rb') as document:
        files = {
            'document': document
        }
        data = {
            'chat_id': chat_id,
            'caption': message,
            'parse_mode': 'HTML'
        }
        response = requests.post(url, files=files, data=data)
        if response.status_code == 200:
            logging.info(f"✅ Sent {document_path} to Telegram successfully.")
        else:
            logging.error(f"❌ Failed to send {document_path} to Telegram.")


def send_telegram_message(message, parse_mode="HTML"):
    url = f"https://api.telegram.org/bot{bot_token}/sendMessage"
    data = {"chat_id": chat_id, "text": message, "parse_mode": parse_mode}
    response = requests.post(url, data=data)
    if response.status_code == 200:
        logging.info("✅ Message sent to Telegram successfully.")
    else:
        logging.error(f"❌ Failed to send message to Telegram. Status: {response.status_code}, Response: {response.text}")

def main():
    parser = argparse.ArgumentParser(description="Send WireGuard configurations.")
    parser.add_argument("-c", "--code", action="store_true", help="Send the configuration as code in the Telegram message.")
    args = parser.parse_args()

    changed_ips = load_changed_ips()
    if changed_ips:
        generate_and_send_wireguard_config(changed_ips, send_as_code=args.code)
    else:
        logging.info("🌈 No IP changes detected requiring WireGuard configuration update. All is well! 🌈")

if __name__ == "__main__":
    main()



