import requests
import time

# Initialize last_ips outside the functions to track IP changes across checks
last_ips = {}

def fetch_locations():
    """Fetch the list of server locations from the API."""
    url = "https://api.uymgg1.com/v4/server/clusters/all?countryCode=CN"
    print("🔍 Fetching the list of server locations...")
    response = requests.get(url)
    if response.status_code == 200:
        locations = response.json()
        print(f"✅ Successfully fetched {len(locations)} locations.")
        return locations
    else:
        print(f"❌ Failed to fetch server locations. Status code: {response.status_code}")
        return []

def assemble_payloads(locations):
    """Assemble a payload for each server location."""
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
    print(f"📦 Payloads assembled for {len(payloads)} locations.")
    return payloads

def send_post_request_and_extract_ip(payload, post_url, headers):
    """Send a POST request with the payload and extract the IP address from the response."""
    response = requests.post(post_url, headers=headers, json=payload)
    if response.status_code == 200:
        ip_address = response.json().get("ip")
        if ip_address:
            print(f"✅ Successfully fetched IP {ip_address} for {payload['location']}.")
            return ip_address
        else:
            print(f"🔍 IP address not found in the response for {payload['location']}.")
            return None
    else:
        print(f"❌ Failed to fetch IP for {payload['location']}. Status code: {response.status_code}")
        return None

def update_server_configurations(payloads):
    """Iterate over payloads, send POST requests, and update configurations based on IP addresses."""
    global last_ips
    post_url = "https://my.surfincn.com/vpn/restricted-cluster"
    headers = {
    "Host": "my.surfincn.com",
    "Cookie": "use yours",
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
    # Designing a header for the table with emojis for a visually appealing notification
    alert_header = "\n              🌟🚨🌟 IP UPDATE ALERT BY FUCKSHARK 🌟🚨🌟\n\n"
    alert_columns = "🌍 Location           | 🔄 Status     | 📌 IP Address        | 🔗 Endpoint\n"
    alert_divider = "-" * 79  # Creating a divider line for aesthetic separation
    alert_message_lines = [alert_header, alert_columns, alert_divider]

    for payload in payloads:
        ip_address = send_post_request_and_extract_ip(payload, post_url, headers)
        location = payload['location']
        endpoint = f"{ip_address}:51820" if ip_address else "Unavailable"
        status = "Changed 🔄" if ip_address and (location not in last_ips or last_ips.get(location) != ip_address) else "Unchanged ✅"

        # Highlight changes for visual emphasis
        if ip_address and (location not in last_ips or last_ips.get(location) != ip_address):
            changes_detected = True

        last_ips[location] = ip_address if ip_address else "Unavailable"

        # Formatting each line to ensure neat alignment
        line_format = f"{location:20} | {status:12} | {ip_address:18} | {endpoint}"
        alert_message_lines.append(line_format)

    if changes_detected:
        alert_message = "\n".join(alert_message_lines)
        print(alert_message)
        send_telegram_message(alert_message)
    else:
        print("🌈 No changes detected in IP addresses. All locations are up-to-date! 🌈")

def main():
    print("🔎 Starting IP address check...")
    locations = fetch_locations()
    if locations:
        payloads = assemble_payloads(locations)
        update_server_configurations(payloads)
    else:
        print("⚠️ No locations found or failed to fetch location information.")
    
    print("\n⏳ Waiting for the next check...")
    time.sleep(1800)  # Delay for 30 minutes before next check

def send_telegram_message(message):
    bot_token = ''
    chat_id = '1289941194'
    send_text = f'https://api.telegram.org/bot{bot_token}/sendMessage?chat_id={chat_id}&parse_mode=Markdown&text={message}'

    response = requests.get(send_text)
    return response.json()

if __name__ == "__main__":
    main()
