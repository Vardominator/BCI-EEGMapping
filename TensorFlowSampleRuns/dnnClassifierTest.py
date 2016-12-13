import tensorflow as tf

import numpy as np
import random
import pandas as pd

random.seed(42)

TRAINING = "trainingset.csv"
TEST = "testset.csv"

titanicDF = pd.read_csv('data/titanic_train.csv')
titanicDF = titanicDF[['Age', 'SibSp', 'Fare', 'Survived']].fillna(0)

titanicDF.Age = titanicDF.Age.astype(int)
titanicDF.SibSp = titanicDF.SibSp.astype(int)
titanicDF.Fare = titanicDF.Fare.astype(int)

titanicDFLength = len(titanicDF)
testLength = int(0.1 * titanicDFLength)
trainingLength = int(0.9 * titanicDFLength)


# Obtain test set
testSetRandomRows = np.random.choice(titanicDF.index.values, testLength)
test = titanicDF.iloc[testSetRandomRows]
test.columns = [str(len(test)), str(3), 'Not survived', 'Survived']
test.to_csv("testset.csv", index=False)

# Obtain training set
trainingSetRandomRows = np.random.choice(titanicDF.index.values, trainingLength)
training = titanicDF.iloc[trainingSetRandomRows]
training.columns = [str(len(training)), str(3), 'Not survived', 'Survived']
training.to_csv("trainingset.csv", index=False)


testSet = tf.contrib.learn.datasets.base.load_csv_with_header(
    filename=TEST, target_dtype=np.int, features_dtype=np.int)

trainingSet = tf.contrib.learn.datasets.base.load_csv_with_header(
    filename=TRAINING, target_dtype=np.int, features_dtype=np.int)


featureColumns = [tf.contrib.layers.real_valued_column("", dimension=3)]


# Build a 3 layer DNN with 10, 20, 10 units respectively
classifier = tf.contrib.learn.DNNClassifier(
                n_classes=3,
                feature_columns=featureColumns,
                hidden_units=[10, 20, 10])


# Fit model
classifier.fit(
                x=trainingSet.data,
                y=trainingSet.target,
                steps=2000)


# Evaluate accuracy
accuracyScore = classifier.evaluate(
                x=testSet.data,
                y=testSet.target)["accuracy"]


print('Accuracy: {0:f}'.format(accuracyScore))

# Predict
predictions = classifier.predict(testSet.data)
print('Predictions: {}'.format(str(predictions)))

