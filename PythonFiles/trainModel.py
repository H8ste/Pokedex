# USAGE
# python train.py --dataset dataset --model pokedex.model --labelbin lb.pickle

# set the matplotlib backend so figures can be saved in the background
import os
import cv2
import pickle
import random
import argparse
import numpy as np
from imutils import paths
import matplotlib.pyplot as plt
from pyimagesearch.smallervggnet import SmallerVGGNet
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import LabelBinarizer
from keras.preprocessing.image import img_to_array
from keras.optimizers import Adam
from keras.preprocessing.image import ImageDataGenerator
from keras import losses
from keras.applications.resnet50 import ResNet50
import matplotlib
matplotlib.use("Agg")

# import the necessary packages

# Sets up the arguments used to run trainModel.py
ap = argparse.ArgumentParser()
ap.add_argument("-d", "--dataset", required=True,
                help="path to the input dataset (i.e., the directory containing the pokemon images)")
ap.add_argument("-m", "--model", required=True,
                help="path where model trained will be saved to")
ap.add_argument("-l", "--labelbin", required=False,
                help="path to output label binarizer produced by sklearn")
ap.add_argument("-p", "--plot", type=str, default="plot.png",
                help="path to output accuracy/loss plot generated by matlab")
args = vars(ap.parse_args())

# number of cycles through the entire training dataset
Epochs = 100
InitialLearningRate = 1e-3  # or 0.001
# the number of training examples utilized in one iteration
BatchSize = 32
# for resizing the image through openCV
ImgDim = (96, 96, 3)

# initializing data and labels
data = []
labels = []

# load the image paths, and shuffle them
print("--Loading images--")

# sorts and loads the image paths into an array
# using module paths from imiage utilities (imutils)
imgPaths = sorted(list(paths.list_images(args["dataset"])))

# answer to everything is 42
random.seed(42)
random.shuffle(imgPaths)

for imgPath in imgPaths:
    # convert image from path to an img object
    img = cv2.imread(imgPath)
    # resizes all the images to be the same, using openCV
    img = cv2.resize(img, (ImgDim[1], ImgDim[2]))
    img = img_to_array(img)
    # adds image to data array
    data.append(img)

    # extracts the folder name and adds as label for each image
    label = imgPath.split(os.path.sep)[-2]
    labels.append(label)

# scale every elements and their values, to be between 0 and 1 using decimals
data = np.array(data, dtype="float") / 255.0
labels = np.array(labels)
print("--Data Matrix :: {:.2f}MB".format(data.nbytes / (1024 * 1000.0)))
print(labels)

# transforms the pokemon names from string to number references, ['yes', 'no', 'no', 'yes'] -> [0, 1, 1, 0]
labelBinarizer = LabelBinarizer()
labels = labelBinarizer.fit_transform(labels)
print(labels)

# splits up the training data into training and testing
# 80% training, 20% testing
# once again, 42 is the answer to everything
(trainDataImgX, testDataImgX, trainDataLabelY, testDataLabelY) = train_test_split(
    data, labels, test_size=0.2, random_state=42)

print("--training Y coordinate :: ", trainDataLabelY)
print("--training X coordinate :: ", trainDataImgX)
print("--test Y coordinate :: ", testDataLabelY)
print("--test X coordinate :: ", testDataImgX)

# https://keras.io/preprocessing/image/#imagedatagenerator-class
# Generates batches of image data to be used with tensor flow (over the epochs)
augmentation = ImageDataGenerator(rotation_range=25, width_shift_range=0.1, height_shift_range=0.1,
                                  shear_range=0.2, zoom_range=0.2, horizontal_flip=True, fill_mode="nearest")

print("--compiling model--")

model = SmallerVGGNet.build(
    width=ImgDim[0], height=ImgDim[1], depth=ImgDim[2], classes=len(labelBinarizer.classes_))

# VIKTOR HELP
# optimize the model (dimensionality)
optimizer = Adam(lr=InitialLearningRate, decay=InitialLearningRate / Epochs)

# VIKTOR HELP
# The penalty "approach" chosen is the categorical crossentropy
model.compile(loss=losses.categorical_crossentropy,
              optimizer=optimizer, metrics=["accuracy"])

print("--training the model--")
trainedNetwork = model.fit_generator(
    aug.flow(trainDataImgX, trainDataLabelY, batch_size=BatchSize),
    validation_data=(testDataImgX, testDataLabelY),
    steps_per_epoch=len(trainDataImgX) // BatchSize,
    epochs=Epochs, verbose=1)


print("--Training done    ,    saving model generated to disk--")
model.save(args["model"])
