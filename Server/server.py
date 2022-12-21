# Programma di Graziano Filippo
# ValtrighExchange 2.0

import requests
import socket
import socketserver
from time import sleep
from threading import Thread

def API():
    # Get the value of ValtrigheCoin form API
    response = requests.get("https://api.coindesk.com/v1/bpi/currentprice.json")
    data_btc = response.json()
    value = data_btc["bpi"]["EUR"]["rate_float"]
    return value


# ValtrigheCoin
def get_vtc():
    while(True):
        value = API()

        # Create a server socket
        server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        server_socket.bind(("", 5000))
        server_socket.listen()

        # Accept a connection from the client
        client_socket, client_address = server_socket.accept()

        # Receive a message from the client
        message = client_socket.recv(1024)
        print("Received:", message.decode())

        # Send a message to the client
        client_socket.send(str(value).encode())

        # Close the client and server sockets
        client_socket.close()
        server_socket.close()

def BuyOrSell():
    value = API()
    while(True):
        # Create a server socket
        server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        server_socket.bind(("", 5001))
        server_socket.listen()

        # Accept a connection from the client
        client_socket, client_address = server_socket.accept()

        # Receive a message from the client
        message2 = client_socket.recv(1024)

        if message2.decode()[:3] == "BUY":
            print("BUY:")
            print("    Number of ValtrigheCoin bought: " + message2.decode()[message2.decode().find(" ") + 1:message2.decode().find(" ", message2.decode().find(" ") + 1)])
            print("    Purchase price: " + str(value))
        elif message2.decode()[:4] == "SELL":
            print("SELL:")
            print("    Number of ValtrigheCoin sold: " + message2.decode()[message2.decode().find(" ") + 1:message2.decode().find(" ", message2.decode().find(" ") + 1)])
            print("    Selling price: " + str(value))
        else:
            print("ERROR")

        # Close the client and server sockets
        client_socket.close()
        server_socket.close()

if __name__ == "__main__":
    thread1 = Thread(target = get_vtc)
    thread2 = Thread(target = BuyOrSell)
    thread1.start()
    thread2.start()