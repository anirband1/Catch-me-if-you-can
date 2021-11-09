import cv2 as cv
import mediapipe as mp
import time
import socket
import sys

# c# communication requirements
connectionRefused = True
quitApp = False
print("Python launched")

try:
    print("Entered try block")
    host, port = "localhost", 9999
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    print("Initialized sock")
    sock.connect((host, port))
    print("Connected to host at port")
    connectionRefused = False
    print("Connectcion Established")
except (ConnectionRefusedError):
    print("Connection Refused")

# hand detection
cap = cv.VideoCapture(0)

mp_hands = mp.solutions.hands
mpDraw = mp.solutions.drawing_utils

hands = mp_hands.Hands(static_image_mode=False,
                       max_num_hands=2,
                       min_detection_confidence=0.75,
                       min_tracking_confidence=0.75)

sPosVector = "0,0,0"

print("Starting main loop")
while True:

    if quitApp:
        break

    time.sleep(1 / 60)

    success, frame = cap.read()
    frame = cv.flip(frame, 1)

    # key = cv.waitKey(1) & 0xFF  # Keyboard input

    frameRGB = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
    #frameHeight, frameWidth, _ = frame.shape
    result = hands.process(frameRGB)
    hand_land_mark = result.multi_hand_landmarks  # hand land marks

    if hand_land_mark:  # hand on screen
        for handLM in hand_land_mark:
            for id, lm in enumerate(handLM.landmark):

                if id == 8:

                    h, w, c = frame.shape
                    # cx, cy = int(lm.x * w), int(lm.y * h)

                    cx, cy = (lm.x), (lm.y)
                    # print(id, cx, cy)

                    cv.circle(frame, (int(cx * w), int(cy * h)), 25,
                              (255, 0, 255), -1)

                    sPosVector = f"{2 * cx - 1},{2 * cy - 1},0"  # position of index finger (scaled to be from -1 to 1)

            mpDraw.draw_landmarks(frame, handLM, mp_hands.HAND_CONNECTIONS)

    # Converting string to Byte, and sending it to C#

    if not connectionRefused:
        sock.sendall(sPosVector.encode("UTF-8"))

        #receiveing data in Byte fron C#, and converting it to String
        receivedData = sock.recv(1024).decode(
            "UTF-8"
        )  # change 1024 to something lower hopefully makes things faster (128 ?)

        if receivedData == "Stop":
            quitApp = True

    cv.imshow("video", frame)

    cv.waitKey(1)

sys.exit()