﻿
using System.Windows.Forms;

namespace Deductions
{
    partial class HomePage
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        private Button GetCreateInvestmentButton()
        {
            return manageInvestmentsButton;
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            AccountLabel = new Label();
            manageInvestmentsButton = new Button();
            accountComboBox = new ComboBox();
            investmentComboBox = new ComboBox();
            InvestmentLabel = new Label();
            createAccountButton = new Button();
            TransactionDataGridView = new DataGridView();
            createTransactionButton = new Button();
            FinancialYearLabel = new Label();
            FinancialYearComboBox = new ComboBox();
            NetValueLabel = new Label();
            DeleteButton = new Button();
            createIncomeButton = new Button();
            createExpenseButton = new Button();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            importCsvToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)TransactionDataGridView).BeginInit();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // AccountLabel
            // 
            AccountLabel.AutoSize = true;
            AccountLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            AccountLabel.Location = new Point(18, 40);
            AccountLabel.Name = "AccountLabel";
            AccountLabel.Size = new Size(66, 21);
            AccountLabel.TabIndex = 1;
            AccountLabel.Text = "Account";
            // 
            // manageInvestmentsButton
            // 
            manageInvestmentsButton.BackColor = Color.GreenYellow;
            manageInvestmentsButton.Location = new Point(160, 102);
            manageInvestmentsButton.Name = "manageInvestmentsButton";
            manageInvestmentsButton.Size = new Size(138, 24);
            manageInvestmentsButton.TabIndex = 2;
            manageInvestmentsButton.Text = "Manage Investments";
            manageInvestmentsButton.UseVisualStyleBackColor = false;
            manageInvestmentsButton.Click += manageInvestmentsButton_Click;
            // 
            // accountComboBox
            // 
            accountComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            accountComboBox.FormattingEnabled = true;
            accountComboBox.Location = new Point(18, 73);
            accountComboBox.Name = "accountComboBox";
            accountComboBox.Size = new Size(121, 23);
            accountComboBox.TabIndex = 3;
            // 
            // investmentComboBox
            // 
            investmentComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            investmentComboBox.FormattingEnabled = true;
            investmentComboBox.Location = new Point(160, 73);
            investmentComboBox.Name = "investmentComboBox";
            investmentComboBox.Size = new Size(138, 23);
            investmentComboBox.TabIndex = 5;
            investmentComboBox.DropDownClosed += investmentComboBox_SelectionChanged;
            // 
            // InvestmentLabel
            // 
            InvestmentLabel.AutoSize = true;
            InvestmentLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            InvestmentLabel.Location = new Point(160, 40);
            InvestmentLabel.Name = "InvestmentLabel";
            InvestmentLabel.Size = new Size(87, 21);
            InvestmentLabel.TabIndex = 4;
            InvestmentLabel.Text = "Investment";
            // 
            // createAccountButton
            // 
            createAccountButton.BackColor = Color.GreenYellow;
            createAccountButton.BackgroundImageLayout = ImageLayout.None;
            createAccountButton.Location = new Point(18, 102);
            createAccountButton.Name = "createAccountButton";
            createAccountButton.Size = new Size(121, 24);
            createAccountButton.TabIndex = 6;
            createAccountButton.Text = "Create Account";
            createAccountButton.UseVisualStyleBackColor = false;
            // 
            // TransactionDataGridView
            // 
            TransactionDataGridView.AllowUserToAddRows = false;
            TransactionDataGridView.AllowUserToDeleteRows = false;
            TransactionDataGridView.AllowUserToOrderColumns = true;
            TransactionDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            TransactionDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            TransactionDataGridView.Location = new Point(18, 141);
            TransactionDataGridView.Name = "TransactionDataGridView";
            TransactionDataGridView.ReadOnly = true;
            TransactionDataGridView.Size = new Size(1052, 382);
            TransactionDataGridView.TabIndex = 7;
            TransactionDataGridView.ColumnHeaderMouseClick += TransactionDataGridView_ColumnHeaderMouseClick;
            // 
            // createTransactionButton
            // 
            createTransactionButton.BackColor = Color.PaleGoldenrod;
            createTransactionButton.Location = new Point(1094, 393);
            createTransactionButton.Name = "createTransactionButton";
            createTransactionButton.Size = new Size(141, 40);
            createTransactionButton.TabIndex = 8;
            createTransactionButton.Text = "Create Transaction";
            createTransactionButton.UseVisualStyleBackColor = false;
            createTransactionButton.Click += createTransactionButton_Click;
            // 
            // FinancialYearLabel
            // 
            FinancialYearLabel.AutoSize = true;
            FinancialYearLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FinancialYearLabel.Location = new Point(710, 40);
            FinancialYearLabel.Name = "FinancialYearLabel";
            FinancialYearLabel.Size = new Size(105, 21);
            FinancialYearLabel.TabIndex = 9;
            FinancialYearLabel.Text = "Financial Year";
            // 
            // FinancialYearComboBox
            // 
            FinancialYearComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            FinancialYearComboBox.FormattingEnabled = true;
            FinancialYearComboBox.Location = new Point(710, 73);
            FinancialYearComboBox.Name = "FinancialYearComboBox";
            FinancialYearComboBox.Size = new Size(121, 23);
            FinancialYearComboBox.TabIndex = 10;
            FinancialYearComboBox.DropDownClosed += financialYearComboBox_SelectionChanged;
            // 
            // NetValueLabel
            // 
            NetValueLabel.AutoSize = true;
            NetValueLabel.Location = new Point(34, 540);
            NetValueLabel.Name = "NetValueLabel";
            NetValueLabel.Size = new Size(58, 15);
            NetValueLabel.TabIndex = 11;
            NetValueLabel.Text = "Summary";
            // 
            // DeleteButton
            // 
            DeleteButton.BackColor = Color.DarkKhaki;
            DeleteButton.Location = new Point(1094, 141);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new Size(133, 49);
            DeleteButton.TabIndex = 12;
            DeleteButton.Text = "Deleted Selected Rows";
            DeleteButton.UseVisualStyleBackColor = false;
            DeleteButton.Click += DeleteTransactions;
            // 
            // createIncomeButton
            // 
            createIncomeButton.BackColor = Color.YellowGreen;
            createIncomeButton.Location = new Point(1094, 439);
            createIncomeButton.Name = "createIncomeButton";
            createIncomeButton.Size = new Size(141, 40);
            createIncomeButton.TabIndex = 13;
            createIncomeButton.Text = "Create Income";
            createIncomeButton.UseVisualStyleBackColor = false;
            createIncomeButton.Click += createIncomeButton_Click;
            // 
            // createExpenseButton
            // 
            createExpenseButton.BackColor = Color.Red;
            createExpenseButton.Location = new Point(1094, 483);
            createExpenseButton.Name = "createExpenseButton";
            createExpenseButton.Size = new Size(141, 40);
            createExpenseButton.TabIndex = 14;
            createExpenseButton.Text = "Create Expense";
            createExpenseButton.UseVisualStyleBackColor = false;
            createExpenseButton.Click += createExpenseButton_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1248, 24);
            menuStrip1.TabIndex = 15;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { importCsvToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // importCsvToolStripMenuItem
            // 
            importCsvToolStripMenuItem.Name = "importCsvToolStripMenuItem";
            importCsvToolStripMenuItem.Size = new Size(130, 22);
            importCsvToolStripMenuItem.Text = "Import csv";
            importCsvToolStripMenuItem.Click += importCsvToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // HomePage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1248, 604);
            Controls.Add(createExpenseButton);
            Controls.Add(createIncomeButton);
            Controls.Add(DeleteButton);
            Controls.Add(NetValueLabel);
            Controls.Add(FinancialYearComboBox);
            Controls.Add(FinancialYearLabel);
            Controls.Add(createTransactionButton);
            Controls.Add(TransactionDataGridView);
            Controls.Add(createAccountButton);
            Controls.Add(investmentComboBox);
            Controls.Add(InvestmentLabel);
            Controls.Add(accountComboBox);
            Controls.Add(manageInvestmentsButton);
            Controls.Add(AccountLabel);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "HomePage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "InvestmentManager";
            ((System.ComponentModel.ISupportInitialize)TransactionDataGridView).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label AccountLabel;
        private Button manageInvestmentsButton;
        private ComboBox accountComboBox;
        private ComboBox investmentComboBox;
        private Label InvestmentLabel;
        private Button createAccountButton;
        private DataGridView TransactionDataGridView;
        private Button createTransactionButton;
        private Label FinancialYearLabel;
        private ComboBox FinancialYearComboBox;
        private Label NetValueLabel;
        private Button DeleteButton;
        private Button createIncomeButton;
        private Button createExpenseButton;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem importCsvToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
    }
}
