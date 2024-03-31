namespace Deductions
{
    partial class ManageInvestments
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
            createInvestmentButton = new Button();
            deleteInvestmentButton = new Button();
            InvestmentsLabel = new Label();
            investmentsListBox = new ListBox();
            doneButton = new Button();
            SuspendLayout();
            // 
            // createInvestmentButton
            // 
            createInvestmentButton.BackColor = SystemColors.Control;
            createInvestmentButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            createInvestmentButton.ForeColor = Color.ForestGreen;
            createInvestmentButton.Location = new Point(433, 73);
            createInvestmentButton.Name = "createInvestmentButton";
            createInvestmentButton.Size = new Size(21, 23);
            createInvestmentButton.TabIndex = 1;
            createInvestmentButton.Text = "+";
            createInvestmentButton.UseVisualStyleBackColor = false;
            createInvestmentButton.Click += createInvestmentButton_Click;
            // 
            // deleteInvestmentButton
            // 
            deleteInvestmentButton.BackColor = SystemColors.Control;
            deleteInvestmentButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            deleteInvestmentButton.ForeColor = Color.Red;
            deleteInvestmentButton.Location = new Point(433, 102);
            deleteInvestmentButton.Name = "deleteInvestmentButton";
            deleteInvestmentButton.Size = new Size(21, 23);
            deleteInvestmentButton.TabIndex = 2;
            deleteInvestmentButton.Text = "-";
            deleteInvestmentButton.UseVisualStyleBackColor = false;
            deleteInvestmentButton.Click += deleteInvestmentButton_Click;
            // 
            // InvestmentsLabel
            // 
            InvestmentsLabel.AutoSize = true;
            InvestmentsLabel.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            InvestmentsLabel.Location = new Point(180, 43);
            InvestmentsLabel.Name = "InvestmentsLabel";
            InvestmentsLabel.Size = new Size(112, 25);
            InvestmentsLabel.TabIndex = 3;
            InvestmentsLabel.Text = "Investments";
            // 
            // investmentsListBox
            // 
            investmentsListBox.FormattingEnabled = true;
            investmentsListBox.ItemHeight = 15;
            investmentsListBox.Location = new Point(180, 73);
            investmentsListBox.Name = "investmentsListBox";
            investmentsListBox.Size = new Size(247, 199);
            investmentsListBox.TabIndex = 4;
            // 
            // doneButton
            // 
            doneButton.Location = new Point(270, 353);
            doneButton.Name = "doneButton";
            doneButton.Size = new Size(75, 23);
            doneButton.TabIndex = 5;
            doneButton.Text = "Done";
            doneButton.UseVisualStyleBackColor = true;
            doneButton.Click += doneButton_Click;
            // 
            // ManageInvestments
            // 
            AcceptButton = doneButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(619, 407);
            Controls.Add(doneButton);
            Controls.Add(investmentsListBox);
            Controls.Add(InvestmentsLabel);
            Controls.Add(deleteInvestmentButton);
            Controls.Add(createInvestmentButton);
            Name = "ManageInvestments";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ManageInvestments";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button createInvestmentButton;
        private Button deleteInvestmentButton;
        private Label InvestmentsLabel;
        private ListBox investmentsListBox;
        private Button doneButton;
    }
}