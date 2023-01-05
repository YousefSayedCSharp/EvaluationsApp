namespace EvaluationsApp;

public partial class frmMain : Form
{
    fbDatabase db = new fbDatabase();

    public frmMain()
    {
        InitializeComponent();
        Run();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
        if (txtName.Text.Trim() == "" && txtMessage.Text.Trim() == "")
        {
            Application.Exit();
        }
        else
        {
            if (MessageBox.Show("توجد بعض الحقول غير فارغة الرجاء التأكيد على عملية الخروج.", "الخروج من البرنامج", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) == DialogResult.Yes)
                Application.Exit();
        }
    }

    private async void btnSend_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtMessage.Text.Trim()))
            {
                MessageBox.Show("حقل كتابة الرسالة فارغ!\nالرجاء كتابة رسالة لإرسالها وإعادة المحاولة.", "كتابة رسالة", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);
                txtMessage.Focus();
                return;
            }

            string name = txtName.Text.Trim();
            string message = txtMessage.Text.Trim();
            await db.AddMessage(message, name);
            Run();
        }catch(Exception ex)
        {
            MessageBox.Show(ex.Message.Replace("valuation",""));
        }
    }

    public async void Run()
    {
        bool b = CheckInternetConnection();
        if (!b)
        { 
            MessageBox.Show("لا يوجد إتصال بالإنترنيت.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading);
            return;
        }

        txtName.Clear();
        txtMessage.Clear();
        txtName.Focus();
        db = null;
        db = new fbDatabase();
        dgv.DataSource = await db.GetAllApplications();
        dgv.ClearSelection();
    }

    public static bool CheckInternetConnection()
    {
        try
        {
            using (var client = new HttpClient())
            using (var stream = client.GetAsync("http://www.google.com").Result)
            {
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    private void txtMessage_TextChanged(object sender, EventArgs e)
    {
        //if (txtMessage.Text.Trim() == "")
            //btnSend.Location= false;
        //else
            //btnSend.Location= true;
    }
}