import tensorflow as tf
import numpy as np
import random
import pandas as pd

import argparse

random.seed(42)

TEST = "C:/Users/barse/Desktop/Github/BCI-EEGMapping/BCI_EEG_FrontEnd_WPF/BCI_EEG_FrontEnd_WPF/data/testset.csv"

parser.add_argument("dataset", type=str, help="dataset to be used")
parser.add_argument("testpercentage", type=float, help="percentage of dataset to test")
parser.add_argument("featurecount", type=int, help="number of features")

args = parser.parse_args()

dataset = pd.read_csv(args.dataset, sep=',')

dfLength = len(dataset)
testLength = int(args.testpercentage/100 * dfLength)

# create test set
testSetRandomRows = np.random.choice(dataset.index.values, testLength)
test = dataset.iloc[testSetRandomRows]
test.to_csv(TEST, index=False, header=False)

# read in test set
testSet = tf.contrib.learn.datasets.base.load_csv_without_header(
    filename=TEST, target_dtype=np.int, features_dtype=np.float32)

featureColumns = [tf.contrib.layers.real_valued_column("", dimension=args.featurecount)]

# build DNN with hidden layers inputted
classifier = tf.contrib.learn.DNNClassifier(
                n_classes=args.classescount,
                feature_columns=featureColumns,
                hidden_units=args.hiddenlayers,
                model_dir="C:/Users/barse/Desktop/Github/BCI-EEGMapping/BCI_EEG_FrontEnd_WPF/BCI_EEG_FrontEnd_WPF/data/currentmodel")


# Evaluate accuracy
accuracyScore = classifier.evaluate(
                x=testSet.data,
                y=testSet.target)["accuracy"]


# Predict
predictions = classifier.predict(testSet.data)
print('Predictions: ', list(predictions))
print('Accuracy: {0:f}'.format(accuracyScore))
