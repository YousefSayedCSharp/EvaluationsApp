using EvaluationsApp.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;

namespace EvaluationsApp;

public class fbDatabase
{
    //new connection and set url
    FirebaseClient firebase ;

    //public ObservableCollection<MessageModel> DatabaseItems { get; set; } = new ObservableCollection<MessageModel>();

    //the model name in firebase project database
    public string tblName { get; set; }

    public fbDatabase()
    {
        firebase = new FirebaseClient("https://appsmanagementactivation-default-rtdb.firebaseio.com/");
        tblName = "Evaluation";
    }

    //get all application and list to get max id
    public async Task<List<MessageModel>> GetAllApplications()
    {
        List<MessageModel> cm = new List<MessageModel>();
        try
        {
            cm = (await firebase
          .Child(tblName)
          .OnceAsync<MessageModel>()).Select(item => new MessageModel
          {
              Id = item.Object.Id,
              Name = item.Object.Name,
              Message = item.Object.Message,
              UserName = item.Object.UserName
          }).ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message.Replace("valuation",""));
            //MyAlert.AlertOK(ex.Message);
        }
        return cm;
    }

    public async Task<bool> AddMessage(string message, string name)
    {
        bool b;
        try
        {
            List<MessageModel> myApps = await GetAllApplications();
            int id = (myApps.Count == 0 || myApps == null) ? 1 : myApps.Max(c => c.Id) + 1;
            string UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            await firebase
              .Child(tblName)
              .PostAsync(new MessageModel()
              {
                  Id = id,
                  Name = name,
                  Message = message,
                  UserName = UserName
              });
            b = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message.Replace("valuation",""));
            b = false;
        }
        return b;
    }
}