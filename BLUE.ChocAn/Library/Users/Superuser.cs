﻿using BLUE.ChocAn.Library.Users.Managers;
using BLUE.ChocAn.Library.Users.Operators;
using BLUE.ChocAn.Library.Users.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users
{
    public class Superuser : User, IProvider, IOperator, IManager
    {
        #region Public Properties

        public override string Username { get { return "superuser"; }}
        public override UserRole CurrentRole { get { return UserRole.Super; } }

        #endregion

        #region Constructors

        public Superuser()
        {
        }

        #endregion

        #region Public Methods

        public bool ValidateMemberCard(Member member)
        {
            member.ActivateCard();
            return true;
        }

        public bool ValidateMemberCard(int memberCardNumber)
        {
            throw new NotImplementedException();
        }

        public void BillChocAn()
        {
            throw new NotImplementedException();
        }

        public void ViewProviderDictionary()
        {
            throw new NotImplementedException();
        }

        public bool AddMember(Member member)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMember(Member member)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMember(int memberNumber)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMember(Member member)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMember(int memberNumber)
        {
            throw new NotImplementedException();
        }

        public bool AddProvider(Provider provider)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProvider(Provider provider)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProvider(int providerNumber)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProvider(Provider provider)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProvider(int providerNumber)
        {
            throw new NotImplementedException();
        }

        public void GenerateMemberReport(bool sendEmail = false)
        {
            throw new NotImplementedException();
        }

        public void GenerateProviderReport(bool sendEmail = false)
        {
            throw new NotImplementedException();
        }

        public void GenerateEFTRecord(bool sendEmail = false)
        {
            throw new NotImplementedException();
        }

        public void GenerateManagersSummary(bool sendEmail = false)
        {
            throw new NotImplementedException();
        }

        public void GenerateAllReports(bool sendEmail = false)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
