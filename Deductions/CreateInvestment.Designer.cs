namespace Deductions
{
    partial class CreateInvestment
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
            confirmCreateInvestment = new Button();
            Account_Label = new Label();
            InvestmentName_Textbox = new TextBox();
            InvestmentName_Label = new Label();
            accountComboBox = new ComboBox();
            SuspendLayout();
            // 
            // confirmCreateInvestment
            // 
            confirmCreateInvestment.Location = new Point(241, 348);
            confirmCreateInvestment.Name = "confirmCreateInvestment";
            confirmCreateInvestment.Size = new Size(75, 23);
            confirmCreateInvestment.TabIndex = 0;
            confirmCreateInvestment.Text = "Create";
            confirmCreateInvestment.UseVisualStyleBackColor = true;
            confirmCreateInvestment.Click += confirmCreateInvestment_Click;
            // 
            // Account_Label
            // 
            Account_Label.AutoSize = true;
            Account_Label.Location = new Point(179, 166);
            Account_Label.Name = "Account_Label";
            Account_Label.Size = new Size(249, 15);
            Account_Label.TabIndex = 2;
            Account_Label.Text = "Select the Account this Investment belongs to";
            // 
            // InvestmentName_Textbox
            // 
            InvestmentName_Textbox.Location = new Point(179, 110);
            InvestmentName_Textbox.Name = "InvestmentName_Textbox";
            InvestmentName_Textbox.Size = new Size(272, 23);
            InvestmentName_Textbox.TabIndex = 4;
            // 
            // InvestmentName_Label
            // 
            InvestmentName_Label.AutoSize = true;
            InvestmentName_Label.Location = new Point(179, 78);
            InvestmentName_Label.Name = "InvestmentName_Label";
            InvestmentName_Label.Size = new Size(183, 15);
            InvestmentName_Label.TabIndex = 5;
            InvestmentName_Label.Text = "Enter a name for your Investment";
            // 
            // accountComboBox
            // 
            accountComboBox.FormattingEnabled = true;
            accountComboBox.Location = new Point(179, 194);
            accountComboBox.Name = "accountComboBox";
            accountComboBox.Size = new Size(121, 23);
            accountComboBox.TabIndex = 6;
            // 
            // CreateInvestment
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 415);
            Controls.Add(accountComboBox);
            Controls.Add(InvestmentName_Label);
            Controls.Add(InvestmentName_Textbox);
            Controls.Add(Account_Label);
            Controls.Add(confirmCreateInvestment);
            Name = "CreateInvestment";
            Text = "CreateInvestment";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button confirmCreateInvestment;
        private Label Account_Label;
        private TextBox InvestmentName_Textbox;
        private Label InvestmentName_Label;
        private ComboBox accountComboBox;
    }
}