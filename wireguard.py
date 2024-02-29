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
bot_token = '7188473194:AAFD_eRDHRdJWRmQ2vQWpPycOhnHjedEmpE'
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


# Generate WireGuard config and send to Telegram with aesthetic enhancements
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
            intro_message = f"🚀 New WireGuard Configuration for {ip_details['connectionName']} 🚀"
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



