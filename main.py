import requests
import time
import logging
import json

logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')

bot_token = ''
chat_id = ''

# Use a JSON file for storing the last known IPs and additional details
ip_file_path = 'last_ips.json'

def load_last_ips():
    """Load last known IPs and details from a JSON file."""
    try:
        with open(ip_file_path, 'r') as file:
            content = file.read()
            # Check if the file is empty
            if not content.strip():
                logging.warning("⚠️ IP file is empty. Assuming first run.")
                return {}
            last_ips = json.loads(content)
        logging.info("📂 Last known IPs loaded successfully.")
    except FileNotFoundError:
        logging.warning("⚠️ IP file not found. Assuming first run.")
        return {}
    except json.JSONDecodeError as e:
        logging.error(f"❌ Error decoding JSON from IP file: {e}. Assuming corrupted file and starting fresh.")
        return {}
    return last_ips


def save_last_ips(last_ips):
    """Save the current IPs and details to a JSON file, including raw emojis."""
    with open(ip_file_path, 'w', encoding='utf-8') as file:
        json.dump(last_ips, file, indent=4, ensure_ascii=False)
    logging.info("💾 Current IPs saved for future comparison.")


def fetch_locations():
    url = "https://api.uymgg1.com/v4/server/clusters/all?countryCode=CN"
    logging.info("🌐 Fetching the list of server locations...")
    try:
        response = requests.get(url)
        response.raise_for_status()  # Raises an error for bad responses
        locations = response.json()
        logging.info(f"📡 Successfully fetched {len(locations)} locations.")
        return locations
    except Exception as e:
        send_telegram_message(f"❌ Error fetching server locations: {e}")
        logging.error(f"❌ Error fetching server locations: {e}")
        return []

def assemble_payloads(locations):
    payloads = []
    for location in locations:
        payload = {
            "country": location.get("country"),
            "countryCode": location.get("countryCode"),
            "region": location.get("region"),
            "regionCode": location.get("regionCode"),
            "load": location.get("load"),
            "id": location.get("id"),
            "coordinates": location.get("coordinates"),
            "info": location.get("info"),
            "type": location.get("type"),
            "location": location.get("location"),
            "connectionName": location.get("connectionName"),
            "pubKey": location.get("pubKey"),
            "tags": location.get("tags"),
            "transitCluster": location.get("transitCluster"),
            "flagUrl": location.get("flagUrl"),
            "userCountry": "CN"
        }
        payloads.append(payload)
    logging.info(f"🔨 Payloads assembled for {len(payloads)} locations.")
    return payloads

def send_post_request_and_extract_ip(payload, post_url, headers):
    try:
        response = requests.post(post_url, headers=headers, json=payload)
        response.raise_for_status()
        ip_address = response.json().get("ip")
        logging.info(f"🌍 IP for {payload['location']}: {ip_address}")
        return ip_address
    except Exception as e:
        send_telegram_message(f"❌ Error fetching IP for {payload['location']}: {e}")
        logging.error(f"❌ Error fetching IP for {payload['location']}: {e}")
        return None

def update_server_configurations(payloads):
    global last_ips
    post_url = "https://my.surfincn.com/vpn/restricted-cluster"
    headers = {
    "Host": "my.surfincn.com",
    "cookkie":
   "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:122.0) Gecko/20100101 Firefox/122.0",
    "Accept": "*/*",
    "Accept-Language": "en-US,en;q=0.5",
    "Accept-Encoding": "gzip, deflate, br",
    "Referer": "https://my.surfincn.com/vpn/manual-setup/main/wireguard/generate-key?restricted=&country=CN",
    "Content-Type": "application/json",
    "Origin": "https://my.surfincn.com",
    "Sec-Fetch-Dest": "empty",
    "Sec-Fetch-Mode": "cors",
    "Sec-Fetch-Site": "same-origin",
    }

    changes_detected = False
    message_lines = ["🚀 <b>IP UPDATE ALERT BY FUCKSHARK</b> 🚀\n"]
    last_ips = load_last_ips()

    for payload in payloads:
        ip_address = send_post_request_and_extract_ip(payload, post_url, headers)
        location = payload['location']

        if location in last_ips and ip_address == last_ips[location].get('ip'):
            status = "🛑 Unchanged"
        else:
            status = "✅ Changed"
            changes_detected = True

        last_ips[location] = {
            'ip': ip_address,
            'pubKey': payload['pubKey'],
            'connectionName': payload['connectionName'],
            'status': status
        }
        message_lines.append(f"<b>{location}</b>: {status} - {ip_address or 'N/A'}")

    if changes_detected:
        send_telegram_message("\n".join(message_lines))
        save_last_ips(last_ips)
    else:
        send_telegram_message("✨ No IP changes detected. All locations are up-to-date. ✨")
        logging.info("✨ No IP changes detected. All locations are up-to-date.")

def send_telegram_message(message):
    url = f"https://api.telegram.org/bot{bot_token}/sendMessage"
    data = {"chat_id": chat_id, "text": message, "parse_mode": "HTML"}
    try:
        response = requests.post(url, data=data)
        if response.status_code == 200:
            logging.info("✅ Telegram message sent successfully.")
        else:
            logging.error(f"❌ Failed to send Telegram message: {response.text}")
    except Exception as e:
        logging.error(f"❌ Failed to send Telegram message: {e}")
    

def main():
    logging.info("🔎 Starting IP address check...")
    locations = fetch_locations()
    if locations:
        payloads = assemble_payloads(locations)
        update_server_configurations(payloads)
    else:
        logging.warning("⚠️ No locations found or failed to fetch location information.")

if __name__ == "__main__":
    main()
