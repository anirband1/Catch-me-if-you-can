import cv2 as cv
import mediapipe as mp
import time
import socket

# c# communication requirements
host, port = "localhost", 9999
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((host, port))

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
    success, frame = cap.read()
    frame = cv.flip(frame, 1)
    rectangle = cv.rectangle(frame, (30, 30), (100, 100), (0, 255, 0),
                             thickness=-1)
    cv.putText(rectangle,
               "Catch me!", (30, 75),
               cv.FONT_HERSHEY_SIMPLEX,
               fontScale=0.5,
               color=(0, 0, 255),
               thickness=2)

    frameRGB = cv.cvtColor(frame, cv.COLOR_BGR2RGB)
    #frameHeight, frameWidth, _ = frame.shape
    result = hands.process(frameRGB)
    hand_land_mark = result.multi_hand_landmarks  # hand land marks
    if hand_land_mark:
        for handLM in hand_land_mark:
            for id, lm in enumerate(handLM.landmark):
                h, w, c = frame.shape
                cx, cy = int(lm.x * w), int(lm.y * h)
                # print(id, cx, cy)

                if id == 8:

                    cv.circle(frame, (cx, cy), 25, (255, 0, 255), -1)
                    sPosVector = f"{cx},{cy},0"  # position of index finger (updates in real time)

                    sock.sendall(
                        sPosVector.encode("UTF-8")
                    )  #Converting string to Byte, and sending it to C#
                    receivedData = sock.recv(1024).decode(
                        "UTF-8"
                    )  #receiveing data in Byte fron C#, and converting it to String
                    # print(receivedData)

                    # time.sleep(0.0166)  # delta time

            mpDraw.draw_landmarks(frame, handLM, mp_hands.HAND_CONNECTIONS)

    cv.imshow("video", frame)

    cv.waitKey(1)