using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCIClientLibCore.Helpers.OCISignerHelper
{
    public class Password : IPasswordFinder
    {
        private readonly char[] password;

        public Password(char[] password) { this.password = password; }

        public char[] GetPassword() { return (char[])password.Clone(); }
    }
}
