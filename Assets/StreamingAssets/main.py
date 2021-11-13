print("importing videocapture")
from cv2 import VideoCapture  # Takes time to import

from cv2 import flip, cvtColor, COLOR_BGR2RGB, circle, imshow, waitKey

print("importing mediapipe")
import mediapipe.python.solutions.hands as hands_solution
import mediapipe.python.solutions.drawing_utils as mp_draw_utils

import time
import socket
import sys

# c# communication requirements
connectionRefused = True
quitApp = False

try:
    host, port = "localhost", 9999
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect((host, port))
    connectionRefused = False
except (ConnectionRefusedError):
    pass

# hand detection
cap = VideoCapture(0)  # cv here

mp_hands = hands_solution
mp_draw = mp_draw_utils

hands = mp_hands.Hands(static_image_mode=False,
                       max_num_hands=2,
                       min_detection_confidence=0.75,
                       min_tracking_confidence=0.75)

sPosVector = "0,0,0"

while True:

    if quitApp:
        break

    time.sleep(1 / 60)  # Rough value of Time.deltaTime in unity

    success, frame = cap.read()
    frame = flip(frame, 1)  # cv here

    # key = cv.waitKey(1) & 0xFF  # Keyboard input

    frameRGB = cvtColor(frame, COLOR_BGR2RGB)  # cv here
    #frameHeight, frameWidth, _ = frame.shape
    result = hands.process(frameRGB)
    hand_land_mark = result.multi_hand_landmarks  # hand land marks

    if hand_land_mark:  # hand on screen
        for handLM in hand_land_mark:
            for id, lm in enumerate(handLM.landmark):

                if id == 8:  # index finger

                    h, w, c = frame.shape
                    # cx, cy = int(lm.x * w), int(lm.y * h)

                    cx, cy = (lm.x), (lm.y)
                    # print(id, cx, cy)

                    circle(frame, (int(cx * w), int(cy * h)), 25,
                           (255, 0, 255), -1)

                    sPosVector = f"{2 * cx - 1},{2 * cy - 1},0"  # position of index finger (scaled to be from -1 to 1)

            mp_draw.draw_landmarks(frame, handLM, mp_hands.HAND_CONNECTIONS)

    # Converting string to Byte, and sending it to C#

    if not connectionRefused:
        sock.sendall(sPosVector.encode("UTF-8"))

        #receiveing data in Byte fron C#, and converting it to String
        receivedData = sock.recv(1024).decode(
            "UTF-8"
        )  # change 1024 to something lower hopefully makes things faster (128 ?)

        if receivedData == "Stop":
            quitApp = True

    imshow("video", frame)

    waitKey(1)

sys.exit()

# put connect block in func, every x milliseconds, call func until connects ?w