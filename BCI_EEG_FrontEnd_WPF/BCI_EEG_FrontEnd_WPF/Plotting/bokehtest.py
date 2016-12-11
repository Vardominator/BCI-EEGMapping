from bokeh.plotting import figure, output_file, show, save

import pandas as pd
import argparse

parser = argparse.ArgumentParser(
    description='Script for plotting NN dataset'
)

parser.add_argument("dataset", type=str, help="dataset to be used")
parser.add_argument("feature1", type=str)
parser.add_argument("feature2", type=str)

args = parser.parse_args()

dataset = pd.read_csv(args.dataset, sep=',')

column1 = dataset[args.feature1]
column2 = dataset[args.feature2]


# output to static HTML file
output_file("C:/xampp/htdocs/blahblah.html")

# create a new plot
p = figure(
   tools="pan,box_zoom,reset,save",
   width = 470,
   height = 307
)

# add some renderers
p.scatter(column1, column2)

# show the results
save(p)