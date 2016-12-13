import tensorflow as tf

import numpy as np
import random
import pandas as pd

random.seed(42)

titanicDF = pd.read_csv('data/titanic_train.csv')
titanicDFLength = len(titanicDF)
testLength = int(0.1 * titanicDFLength)
trainingLength = int(0.9 * titanicDFLength)

titanicX = titanicDF[['Age', 'SibSp', 'Fare', 'Survived']].fillna(0)
titanicY = titanicDF['Survived'];

# Obtain test set
testSetRandomRows = np.random.choice(titanicDF.index.values, testLength)
testX = titanicX.iloc[testSetRandomRows]
testY = titanicY.iloc[testSetRandomRows]

# Obtain training set
trainingSetRandomRows = np.random.choice(titanicDF.index.values, trainingLength)
trainingX = titanicX.iloc[trainingSetRandomRows]
trainingY = titanicY.iloc[trainingSetRandomRows]

classifier = tf.contrib.learn.LinearClassifier(n_classes=2,
    feature_columns=tf.contrib.learn.infer_real_valued_columns_from_input(trainingX),
    optimizer=tf.train.GradientDescentOptimizer(learning_rate=0.05))

classifier.fit(trainingX, trainingY, batch_size=50, steps=1000)

predictions = classifier.predict(testX)

score = classifier.evaluate(testX, testY)["accuracy"]

print(score)
print('Predictions: {}'.format(str(predictions)))
