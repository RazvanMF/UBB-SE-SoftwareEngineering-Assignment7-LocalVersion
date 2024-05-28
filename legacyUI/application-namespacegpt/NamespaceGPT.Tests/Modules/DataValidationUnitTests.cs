using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamespaceGPT.Common.BasicDataValidation.Module.Implementations.Sanitzators;
using NamespaceGPT.Common.BasicDataValidation.Module.Implementations.Validators;
using NamespaceGPT.Common.Modules.BasicDataValidation.Module.Implementations.Validators;

namespace NamespaceGPT.Tests.Modules
{
    [TestClass]
    public class DataValidationUnitTests
    {
        [TestMethod]
        public void TestUserNameValidator()
        {
            UsernameValidator validator = new UsernameValidator();

            Debug.Assert(validator.Validate("valid_username27"));

            Debug.Assert(!validator.Validate(string.Empty));
            Debug.Assert(!validator.Validate(null));
            Debug.Assert(!validator.Validate(" "));
            Debug.Assert(!validator.Validate("ab"));
            Debug.Assert(!validator.Validate("1a2b3c4d5e6f7g8h9i1a2b3c4d5e6f7g8h9i"));
            Debug.Assert(!validator.Validate("username with space"));
        }

        [TestMethod]
        public void TestPasswordValidator()
        {
            PasswordValidator validator = new PasswordValidator();

            Debug.Assert(!validator.Validate(string.Empty));
            Debug.Assert(!validator.Validate(null));
            Debug.Assert(!validator.Validate(" "));
            Debug.Assert(!validator.Validate("Short_1"));
            Debug.Assert(!validator.Validate("no_upper_case1"));
            Debug.Assert(!validator.Validate("NO_LOWER_CASE2"));
            Debug.Assert(!validator.Validate("no_numbers"));
            Debug.Assert(!validator.Validate("no1special1char"));
        }

        [TestMethod]
        public void TestURLValidator()
        {
            URLValidator validator = new URLValidator();

            Debug.Assert(validator.Validate("https://www.google.com"));
            Debug.Assert(validator.Validate("http://www.google.com"));

            Debug.Assert(!validator.Validate(string.Empty));
            Debug.Assert(!validator.Validate(null));
            Debug.Assert(!validator.Validate(" "));
            Debug.Assert(!validator.Validate("www.google.com"));
            Debug.Assert(!validator.Validate("invalid url"));
        }

        [TestMethod]
        public void TestPhoneNumberValidator()
        {
            PhoneNumberValidator validator = new PhoneNumberValidator();

            Debug.Assert(validator.Validate("077-123-1234"));
            Debug.Assert(validator.Validate("077 123 1234"));
            Debug.Assert(validator.Validate("077.123.1234"));

            Debug.Assert(!validator.Validate("0771.123.1234"));
            Debug.Assert(!validator.Validate("0771.23.1234"));
            Debug.Assert(!validator.Validate("077.1231.234"));

            Debug.Assert(!validator.Validate(string.Empty));
            Debug.Assert(!validator.Validate(null));
            Debug.Assert(!validator.Validate(" "));
            Debug.Assert(!validator.Validate("something"));
        }

        [TestMethod]
        public void TestEmailValidator()
        {
            EmailValidator validator = new EmailValidator();

            Debug.Assert(validator.Validate("an_example27@unit.test"));

            Debug.Assert(!validator.Validate("an_example27@missing_dot"));
            Debug.Assert(!validator.Validate("an_example27.missing_at"));
            Debug.Assert(!validator.Validate("an_example27_no_special_chars"));

            Debug.Assert(!validator.Validate(string.Empty));
            Debug.Assert(!validator.Validate(null));
            Debug.Assert(!validator.Validate(" "));
            Debug.Assert(!validator.Validate("invalid address"));
        }

        [TestMethod]
        public void TestDateValidator()
        {
            DateValidator validator = new DateValidator();

            Debug.Assert(validator.Validate("12/12/2001"));
            Debug.Assert(validator.Validate("12-12-2001"));
            Debug.Assert(validator.Validate("2001-12-12"));

            Debug.Assert(!validator.Validate("13/13/2001"));
            Debug.Assert(!validator.Validate("13-13-2001"));
            Debug.Assert(!validator.Validate("2001-13-13"));

            Debug.Assert(!validator.Validate(string.Empty));
            Debug.Assert(!validator.Validate(null));
            Debug.Assert(!validator.Validate(" "));
            Debug.Assert(!validator.Validate("something"));
        }

        [TestMethod]
        public void TestInputSanitizer()
        {
            InputSanitizer sanitizer = new InputSanitizer();

            Debug.Assert(sanitizer.Sanitize(null) == null);
            Debug.Assert(sanitizer.Sanitize("'") == "''");
            Debug.Assert(sanitizer.Sanitize("something") == "something");
            Debug.Assert(sanitizer.Sanitize("-- some comment; SELECT * FROM TABLE; /* 'malicious code here' */ EXEC xp_something") == " some comment SELECT * FROM TABLE  ''malicious code here''  EXEC something");
        }
    }
}
