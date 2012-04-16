﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine;

namespace UI
{
    internal sealed class Controller
    {
        public event EventHandler ContactStoreChanged;
        public event EventHandler CivServerChanged;

        public ContactStore ContactStore {get;private set;}
        public CivServer CivServer { get;private set;}

        public void OpenLog()
        {
            // Get the login details
            using (LogonForm lf = new LogonForm())
            {
                DialogResult dr = lf.ShowDialog();
                if (dr != DialogResult.OK)
                    return;

                ContactStore = new ContactStore(lf.Server, lf.Database, lf.Username, lf.Password);
                if (ContactStoreChanged != null)
                    ContactStoreChanged(this, new EventArgs());

                if (!string.IsNullOrEmpty(lf.CivSerialPort))
                {
                    CivServer = new CivServer(lf.CivSerialPort, lf.CivDtr, lf.CivRts);
                    if (CivServerChanged != null)
                        CivServerChanged(this, new EventArgs());

                    // Force a frequency query
                    CivServer.QueryFrequency();
                }
            }
        }
    }
}