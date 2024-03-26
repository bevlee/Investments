namespace Deductions
{
    partial class CreateTransaction
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            createTransactionButton = new Button();
            TransactionValueLabel = new Label();
            TransactionValueTextBox = new TextBox();
            investmentComboBox = new ComboBox();
            categoryLabel = new Label();
            categoryTextBox = new TextBox();
            InvestmentLabel = new Label();
            TransactionTypeLabel = new Label();
            TransactionTypeComboBox = new ComboBox();
            TransactionDateLabel = new Label();
            TransactionDatePicker = new DateTimePicker();
            SuspendLayout();
            // 
            // createTransactionButton
            // 
            createTransactionButton.Location = new Point(168, 418);
            createTransactionButton.Name = "createTransactionButton";
            createTransactionButton.Size = new Size(75, 23);
            createTransactionButton.TabIndex = 0;
            createTransactionButton.Text = "Create";
            createTransactionButton.UseVisualStyleBackColor = true;
            createTransactionButton.Click += createTransactionButton_Click;
            // 
            // TransactionValueLabel
            // 
            TransactionValueLabel.AutoSize = true;
            TransactionValueLabel.Location = new Point(83, 197);
            TransactionValueLabel.Name = "TransactionValueLabel";
            TransactionValueLabel.Size = new Size(182, 15);
            TransactionValueLabel.TabIndex = 9;
            TransactionValueLabel.Text = "Enter a value for your Transaction";
            // 
            // TransactionValueTextBox
            // 
            TransactionValueTextBox.Location = new Point(83, 225);
            TransactionValueTextBox.Name = "TransactionValueTextBox";
            TransactionValueTextBox.Size = new Size(272, 23);
            TransactionValueTextBox.TabIndex = 8;
            // 
            // investmentComboBox
            // 
            investmentComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            investmentComboBox.FormattingEnabled = true;
            investmentComboBox.Location = new Point(83, 79);
            investmentComboBox.Name = "investmentComboBox";
            investmentComboBox.Size = new Size(272, 23);
            investmentComboBox.TabIndex = 14;
            // 
            // categoryLabel
            // 
            categoryLabel.AutoSize = true;
            categoryLabel.Location = new Point(83, 126);
            categoryLabel.Name = "categoryLabel";
            categoryLabel.Size = new Size(184, 15);
            categoryLabel.TabIndex = 13;
            categoryLabel.Text = "Enter a name for your Transaction";
            // 
            // categoryTextBox
            // 
            categoryTextBox.Location = new Point(83, 158);
            categoryTextBox.Name = "categoryTextBox";
            categoryTextBox.Size = new Size(272, 23);
            categoryTextBox.TabIndex = 12;
            // 
            // InvestmentLabel
            // 
            InvestmentLabel.AutoSize = true;
            InvestmentLabel.Location = new Point(83, 51);
            InvestmentLabel.Name = "InvestmentLabel";
            InvestmentLabel.Size = new Size(264, 15);
            InvestmentLabel.TabIndex = 11;
            InvestmentLabel.Text = "Select the Investment this Transaction belongs to";
            // 
            // TransactionTypeLabel
            // 
            TransactionTypeLabel.AutoSize = true;
            TransactionTypeLabel.Location = new Point(83, 262);
            TransactionTypeLabel.Name = "TransactionTypeLabel";
            TransactionTypeLabel.Size = new Size(135, 15);
            TransactionTypeLabel.TabIndex = 16;
            TransactionTypeLabel.Text = "Select a transaction type";
            // 
            // TransactionTypeComboBox
            // 
            TransactionTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            TransactionTypeComboBox.FormattingEnabled = true;
            TransactionTypeComboBox.Location = new Point(83, 289);
            TransactionTypeComboBox.Name = "TransactionTypeComboBox";
            TransactionTypeComboBox.Size = new Size(272, 23);
            TransactionTypeComboBox.TabIndex = 17;
            // 
            // TransactionDateLabel
            // 
            TransactionDateLabel.AutoSize = true;
            TransactionDateLabel.Location = new Point(83, 326);
            TransactionDateLabel.Name = "TransactionDateLabel";
            TransactionDateLabel.Size = new Size(160, 15);
            TransactionDateLabel.TabIndex = 18;
            TransactionDateLabel.Text = "Set a date for this transaction";
            // 
            // TransactionDatePicker
            // 
            TransactionDatePicker.Location = new Point(83, 353);
            TransactionDatePicker.Name = "TransactionDatePicker";
            TransactionDatePicker.Size = new Size(272, 23);
            TransactionDatePicker.TabIndex = 19;
            // 
            // CreateTransaction
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(442, 492);
            Controls.Add(TransactionDatePicker);
            Controls.Add(TransactionDateLabel);
            Controls.Add(TransactionTypeComboBox);
            Controls.Add(TransactionTypeLabel);
            Controls.Add(investmentComboBox);
            Controls.Add(categoryLabel);
            Controls.Add(categoryTextBox);
            Controls.Add(InvestmentLabel);
            Controls.Add(TransactionValueLabel);
            Controls.Add(TransactionValueTextBox);
            Controls.Add(createTransactionButton);
            Name = "CreateTransaction";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CreateTransaction";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button createTransactionButton;
        private Label TransactionValueLabel;
        private TextBox TransactionValueTextBox;
        private ComboBox investmentComboBox;
        private Label categoryLabel;
        private TextBox categoryTextBox;
        private Label InvestmentLabel;
        private Label TransactionTypeLabel;
        private ComboBox TransactionTypeComboBox;
        private Label TransactionDateLabel;
        private DateTimePicker TransactionDatePicker;
    }
}