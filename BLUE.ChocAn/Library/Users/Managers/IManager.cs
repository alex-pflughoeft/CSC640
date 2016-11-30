using BLUE.ChocAn.Library.Communication;
using BLUE.ChocAn.Library.Database.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users.Managers
{
    internal interface IManager
    {
        string GenerateMemberReport(DBHelper dbHelper, EmailSender emailSender = null, bool saveFile = false);
        string GenerateProviderReport(DBHelper dbHelper, EmailSender emailSender = null, bool saveFile = false);
        string GenerateEFTRecord(DBHelper dbHelper, EmailSender emailSender = null, bool saveFile = false);
        string GenerateManagersSummary(DBHelper dbHelper, EmailSender emailSender = null, bool saveFile = false);
        string GenerateAllReports(DBHelper dbHelper, EmailSender emailSender = null, bool saveFile = false);
    }
}