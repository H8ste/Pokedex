#
#   Hello World server in Python
#   Binds REP socket to tcp://*:5555
#   Expects a Pokemon name from client, replies with prediction results
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

    # load the trained convolutional neural network and the label
    json_file = open(os.getcwd() + "\\PythonFiles" + "\\models\\" + model + ".json", 'r')
    loaded_model_json = json_file.read()
    json_file.close()
    loaded_model = model_from_json(loaded_model_json)
   
    # load weights into new model
    loaded_model.load_weights(os.getcwd() + "\\PythonFiles" + "\\models\\" + model + ".h5")


    # load labels
    lb = pickle.loads(open(labels, "rb").read())


    # load the image
    os.chdir(basePath)
    os.chdir(os.getcwd() + "/UnityProject/Assets/Resources/Textures/")
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

    # checks againts the predicted label and actual label given from the file name
    inputFileName = imageClassify
    filename = inputFileName[:-3]

    #  refind returns -1 if string doens't contain argument provided
    correct = "correct" if filename.lower().rfind(label) != -1 else "incorrect"

    # build the label
    label = "{}: {:.2f}% ({})".format(label, proba[idx] * 100, correct)

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
    results = classify("pokedex", os.getcwd() + "\\PythonFiles\\" + "labels/pokedex.pickle", message)

    os.chdir(basePath)

    socket.send_string(results)
    