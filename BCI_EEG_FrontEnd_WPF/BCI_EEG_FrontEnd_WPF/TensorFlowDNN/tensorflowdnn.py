import tensorflow as tf
import numpy as np
import random
import pandas as pd

import argparse

random.seed(42)



TRAINING = "C:/Users/barse/Desktop/Github/BCI-EEGMapping/BCI_EEG_FrontEnd_WPF/BCI_EEG_FrontEnd_WPF/data/trainingset.csv"
TEST = "C:/Users/barse/Desktop/Github/BCI-EEGMapping/BCI_EEG_FrontEnd_WPF/BCI_EEG_FrontEnd_WPF/data/testset.csv"

parser = argparse.ArgumentParser(
    description='Script for running neural network in TensorFlow'
)

parser.add_argument("dataset", type=str, help="dataset to be used")
parser.add_argument("featurecount", type=int, help="number of features")
parser.add_argument("classescount", type=int, help="number of classifications")
parser.add_argument("batchsize", type=int, help="batch size of minimization")
parser.add_argument("steps", type=int, help="the number of iterations for training")
parser.add_argument("-l", "--hls", nargs='+', type=int)

args = parser.parse_args() 

dataset = pd.read_csv(args.dataset, sep=',')


dfLength = len(dataset)

# create training set
trainingSetRandomRows = np.random.choice(dataset.index.values, dfLength)
training = dataset.iloc[trainingSetRandomRows]
training.to_csv(TRAINING, index=False, header=False)


# read in training set
trainingSet = tf.contrib.learn.datasets.base.load_csv_without_header(
    filename=TRAINING, target_dtype=np.int, features_dtype=np.float32)


featureColumns = [tf.contrib.layers.real_valued_column("", dimension=args.featurecount)]


# build DNN with hidden layers inputted
classifier = tf.contrib.learn.DNNClassifier(
                n_classes=args.classescount,
                feature_columns=featureColumns,
                hidden_units=args.hls,
                model_dir="C:/Users/barse/Desktop/Github/BCI-EEGMapping/BCI_EEG_FrontEnd_WPF/BCI_EEG_FrontEnd_WPF/data/currentmodel")


# train DNN
classifier.fit(
                x=trainingSet.data,
                y=trainingSet.target,
                batch_size=args.batchsize,
                steps=args.steps)


f = open('testfile.txt', 'w')
f.write('this is a test\n')
f.close()