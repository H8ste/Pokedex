# USAGE
# python classify.py --model pokedex.model --labelbin lb.pickle --image examples/charmander_counter.png

# import the necessary packages
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

# construct the argument parse and parse the arguments
ap = argparse.ArgumentParser()
ap.add_argument("-m", "--model", required=False,
	help="path to trained model model")
ap.add_argument("-l", "--labelbin", required=True,
	help="path to label binarizer")
ap.add_argument("-i", "--image", required=True,
	help="path to input image")
args = vars(ap.parse_args())


# load the trained convolutional neural network and the label
# binarizer

# load json and create model
json_file = open("pythonfiles/models/" + args["model"] + ".json", 'r')
loaded_model_json = json_file.read()
json_file.close()

loaded_model = model_from_json(loaded_model_json)
# load weights into new model
loaded_model.load_weights("pythonfiles/models/" + args["model"] + ".h5")
print("Loaded model from disk")

# print("[INFO] loading network...")
# model = load_model(args["model"])
lb = pickle.loads(open(args["labelbin"], "rb").read())

allImgPaths = os.listdir(args["image"])

for i in range(len(allImgPaths)):
	# load the image
	print("OSPATH: ", allImgPaths[i])
	image = cv2.imread(("pythonfiles/testimage/" + allImgPaths[i]))
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
	filename = allImgPaths[i][args["image"].rfind(os.path.sep) + 1:]
	print("filename: ", filename)
	# filename = allImgPaths[i]
	correct = "correct" if filename.rfind(label.split("/")[1]) != -1 else "incorrect"

	# build the label and draw the label on the image
	label = "{}: {:.2f}% ({})".format(label, proba[idx] * 100, correct)
	output = imutils.resize(output, width=450)
	cv2.putText(output, label, (10, 25),  cv2.FONT_HERSHEY_SIMPLEX,
		0.6, (0, 255, 0), 2)

	# show the output image
	print("[INFO] {}".format(label))
	cv2.imshow(filename, output)
	cv2.waitKey(0)