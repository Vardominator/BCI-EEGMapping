from tensorflow.contrib import learn

import numpy as np
import random
import pandas as pd

random.seed(42)


titanicDF = pd.read_csv('data/titanic_train.csv')
titanicDFLength = len(titanicDF)

testLength = int(0.1 * titanicDFLength)
trainingLength = int(0.9 * titanicDFLength)

testSetRandomRows = np.random.choice(titanicDF.index.values, testLength)
testSet = titanicDF.ix[testSetRandomRows]

print(testSet.head())

trainingSetRanomRows = np.random.choice(titanicDF.index.values, trainingLength)