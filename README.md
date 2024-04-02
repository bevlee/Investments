Winform application to track income and expenses for Investments.

This application can be used to view transactions by financial year to help with tax returns.

Transactions can be entered manually or by importing from a csv file.

Uses a sqlite db stored in the %APPDATA%/Deductions directory.

## Importing from csv

The csv MUST be in the following format(example file provided as template.csv):

Date, TransactionType, Amount, Category, Note (optional)


The standalone bin is stored as Deductions.zip

## Generating a report

A pdf report can be generated to summarise the currently selected FY and investment.
An example of the generated pdf can be seen below:
![example_pdf](Deductions/resources/examplepdf.png)


Coming up:
- store the source of a document within the app as a reference

