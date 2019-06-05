#
#   Hello World server in Python
#   Binds REP socket to tcp://*:5555
#   Expects b"Hello" from client, replies with b"World"
#
import os
import time
import zmq
import sys
import subprocess

from keras.preprocessing.image import img_to_array
from keras.models import load_model
import numpy as np
import argparse
import imutils
import pickle
import cv2
import os
from keras.models import Sequential
from keras.layers import Dense
from keras.models import model_from_json

def classify(model, labels, imageClassify):
    # # construct the argument parse and parse the arguments
    # ap = argparse.ArgumentParser()
    # ap.add_argument("-m", "--model", required=False,
    #     help="path to trained model model")
    # ap.add_argument("-l", "--labelbin", required=True,
    #     help="path to label binarizer")
    # ap.add_argument("-i", "--image", required=True,
    #     help="path to input image")
    # args = vars(ap.parse_args())


    # load the trained convolutional neural network and the label
    # binarizer

    # load json and create model
    print('currentpath: ' + os.getcwd())
    print('path: ' + os.getcwd() + "\\PythonFiles"+ "\\models\\" + model + ".json")
    json_file = open(os.getcwd() + "\\PythonFiles" + "\\models\\" + model + ".json", 'r')
    loaded_model_json = json_file.read()
    json_file.close()

    loaded_model = model_from_json(loaded_model_json)
    # load weights into new model
    loaded_model.load_weights(os.getcwd() + "\\PythonFiles" + "\\models\\" + model + ".h5")
    print("Loaded model from disk")

    # print("[INFO] loading network...")
    # model = load_model(args["model"])
    lb = pickle.loads(open(labels, "rb").read())


    # load the image
    os.chdir(basePath)
    # os.chdir("..")
    print("current directory: " + os.getcwd())
    os.chdir(os.getcwd() + "/UnityProject/Assets/Resources/Textures/")
    print("current directory: " + os.getcwd())
    image = cv2.imread((imageClassify + ".jpg"))
    output = image.copy()							

    # pre-process the image for classification
    image = cv2.resize(image, (96, 96))
    image = image.astype("float") / 255.0
    image = img_to_array(image)
    image = np.expand_dims(image, axis=0)

    # classify the input image
    print("[INFO] classifying image...")
    proba = loaded_model.predict(image)[0]
    idx = np.argmax(proba)
    label = lb.classes_[idx]

    print("possible labels: ", lb.classes_)

    # we'll mark our prediction as "correct" of the input image filename
    # contains the predicted label text (obviously this makes the
    # assumption that you have named your testing image files this way)
    inputFileName = imageClassify
    filename = inputFileName[:-3]
    print("filename: ", filename)
    #  refind returns -1 if strin doens't contain argument provided
    correct = "correct" if filename.lower().rfind(label.split("/")[1]) != -1 else "incorrect"

    # build the label and draw the label on the image
    label = "{}: {:.2f}% ({})".format(label, proba[idx] * 100, correct)
    # output = imutils.resize(output, width=450)
    # cv2.putText(output, label, (10, 25),  cv2.FONT_HERSHEY_SIMPLEX,
        # 0.6, (0, 255, 0), 2)

    # show the output image
    print("[INFO] {}".format(label))
    # cv2.imshow(filename, output)
    # cv2.waitKey(0)
    return label

context = zmq.Context()
socket = context.socket(zmq.REP)
socket.bind("tcp://*:5555")
print ("server running on port 5555")
basePath = os.getcwd()
while True:

    os.chdir(basePath)
    #  Wait for next request from client
    message = socket.recv_string()
    print("message from server: ", message)
    print("printing results")
    results = classify("longerEpoch", os.getcwd() + "\\PythonFiles\\" + "labels/longerEpoch.pickle", message)
    # print(test)

    #  In the real world usage, you just need to replace time.sleep() with
    #  whatever work you want python to do.
    time.sleep(1)
    os.chdir(basePath)

    #  Send reply back to client
    #  In the real world usage, after you finish your work, send your output here
    socket.send_string(results)

    print("i'm done")

    