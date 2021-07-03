import cv2 as cv
import mediapipe as mp
import time
import socket

# c# communication requirements
connectionRefused = True

try:
    host, port = "localhost", 9999
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect((host, port))
    connectionRefused = False
except (ConnectionRefusedError):
    pass

# hand detection
cap = cv.VideoCapture(0)

mp_hands = mp.solutions.hands
mpDraw = mp.solutions.drawing_utils

hands = mp_hands.Hands(static_image_mode=False,
                       max_num_hands=2,
                       min_detection_confidence=0.75,
                       min_tracking_confidence=0.75)

sPosVector = "0,0,0"

while True:

    time.sleep(0.0166)

    success, frame = cap.read()
    frame = cv.flip(frame, 1)
    # rectangle = cv.rectangle(frame, (30, 30), (100, 100), (0, 255, 0),
    #                          thickness=-1)
    # cv.putText(rectangle,
    #            "Catch me!", (30, 75),
    #            cv.FONT_HERSHEY_SIMPLEX,
    #            fontScale=0.5,
    #            color=(0, 0, 255),
    #            thickness=2)

    frameRGB = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
    #frameHeight, frameWidth, _ = frame.shape
    result = hands.process(frameRGB)
    hand_land_mark = result.multi_hand_landmarks  # hand land marks

    if hand_land_mark:  # hand on screen
        for handLM in hand_land_mark:
            for id, lm in enumerate(handLM.landmark):

                # Right now it sends values relative to 1280x720
                # Should change to sending factor (ignore h, w, c)

                h, w, c = frame.shape
                # cx, cy = int(lm.x * w), int(lm.y * h)

                cx, cy = (lm.x), (lm.y)
                # print(id, cx, cy)

                if id == 8:

                    cv.circle(frame, (int(cx * w), int(cy * h)), 25,
                              (255, 0, 255), -1)

                    sPosVector = f"{1-((1-cx)*2)},{1-((1-cy)*2)},0"  # position of index finger (scaled to be from -1 to 1)

                    # receivedData = sock.recv(1024).decode("UTF-8")  #receiveing data in Byte fron C#, and converting it to String
                    # print(receivedData)

            mpDraw.draw_landmarks(frame, handLM, mp_hands.HAND_CONNECTIONS)

    #Converting string to Byte, and sending it to C#

    if not connectionRefused:
        sock.sendall(sPosVector.encode("UTF-8"))

    cv.imshow("video", frame)

    cv.waitKey(1)