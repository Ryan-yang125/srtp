from keras.preprocessing.image import img_to_array
import imutils
from cv2 import cv2
from keras.models import load_model
import numpy as np
import json
# parameters for loading data and images
detection_model_path = 'models/haarcascade_frontalface_default.xml'
emotion_model_path = 'models/_mini_XCEPTION.102-0.66.hdf5'

# hyper-parameters for bounding boxes shape
# loading models
face_detection = cv2.CascadeClassifier(detection_model_path)
emotion_classifier = load_model(emotion_model_path, compile=False)
EMOTIONS = ["angry", "disgust", "scared",
            "happy", "sad", "surprised", "neutral"]
path = '../Assets/test.png'

while True:
    frame = cv2.imread(path)
    if frame is None:
        print('Reading while writing')
        continue
    # resize the frame
    frame = imutils.resize(frame, width=300)
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    faces = face_detection.detectMultiScale(
        gray, scaleFactor=1.1, minNeighbors=5, minSize=(30, 30), flags=cv2.CASCADE_SCALE_IMAGE)
    canvas = np.zeros((250, 300, 3), dtype="uint8")

    if len(faces) is 0:
        continue

    faces = sorted(faces, reverse=True,
                   key=lambda x: (x[2] - x[0]) * (x[3] - x[1]))[0]
    (fX, fY, fW, fH) = faces
    # Extract the ROI of the face from the grayscale image, resize it to a fixed 28x28 pixels, and then prepare
    # the ROI for classification via the CNN
    roi = gray[fY:fY + fH, fX:fX + fW]
    roi = cv2.resize(roi, (64, 64))
    roi = roi.astype("float") / 255.0
    roi = img_to_array(roi)
    roi = np.expand_dims(roi, axis=0)

    preds = emotion_classifier.predict(roi)[0]
    emotion_probability = np.max(preds)
    label = EMOTIONS[preds.argmax()]
    labels = {"datas": []}
    for (i, (emotion, prob)) in enumerate(zip(EMOTIONS, preds)):
        # construct the label text
        label = {
            "name": emotion,
            "value": "{:.2f}".format(prob)
        }
        labels['datas'].append(label)
    with open('../Assets/test.json', 'w') as fp:
        json.dump(labels, fp)
