using Florists.Application.Interfaces.Services;
using Florists.Infrastructure.Services;

namespace Florists.Test.Unit.Services
{
  public class PasswordServiceTests
  {
    private IPasswordService _cut;

    [SetUp]
    public void Setup()
    {
      _cut = new PasswordService();
    }

    [TestCase("hello")]
    [TestCase("xep624")]
    [TestCase("P@s5w0Rd")]
    public void GenerateHash_Returns60LengthString_WhenAnyInputProvided(string password)
    {
      var hash = _cut.GenerateHash(password);

      Assert.That(hash, Has.Length.EqualTo(60));
    }

    [TestCase("hello")]
    [TestCase("xep624")]
    [TestCase("P@s5w0Rd")]
    public void GenerateHash_Returns13WorkFactor_WhenAnyInputProvided(string password)
    {
      var hash = _cut.GenerateHash(password);

      var actual = hash.Substring(3, 3);

      var expected = "$13";

      Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase("hello")]
    [TestCase("xep624")]
    [TestCase("P@s5w0Rd")]
    public void IsValid_ReturnsTrue_WhenValidPasswordProvided(string password)
    {
      var hash = _cut.GenerateHash(password);

      var actual = _cut.IsValid(password, hash);

      Assert.That(actual, Is.True);
    }

    [TestCase("hello")]
    [TestCase("xep624")]
    [TestCase("P@s5w0Rd")]
    public void IsValid_ReturnsFalse_WhenInvalidPasswordProvided(string password)
    {
      var hash = _cut.GenerateHash(password);

      var actual = _cut.IsValid(password + "aaa", hash);

      Assert.That(actual, Is.False);
    }

    [TestCase("abc123")]
    [TestCase("xep624")]
    [TestCase("P@s5w0Rd")]
    public void IsDigit_ReturnsTrue_WhenDigitPresent(string password)
    {
      var actual = _cut.IsDigit(password);

      Assert.That(actual, Is.True);
    }

    [TestCase("hello")]
    [TestCase("world")]
    [TestCase("xep")]
    public void IsDigit_ReturnsFalse_WhenDigitNotPresent(string password)
    {
      var actual = _cut.IsDigit(password);

      Assert.That(actual, Is.False);
    }

    [TestCase("abc123")]
    [TestCase("xep624")]
    [TestCase("pa5w0rd")]
    public void IsLowercase_ReturnsTrue_WhenLowercasePresent(string password)
    {
      var actual = _cut.IsLowercase(password);

      Assert.That(actual, Is.True);
    }

    [TestCase("HELLO")]
    [TestCase("WORLD")]
    [TestCase("123")]
    public void IsLowercase_ReturnsFalse_WhenLowercaseNotPresent(string password)
    {
      var actual = _cut.IsLowercase(password);

      Assert.That(actual, Is.False);
    }

    [TestCase("HELLO")]
    [TestCase("WORLD")]
    [TestCase("H11da")]
    public void IsUppercase_ReturnsTrue_WhenUppercasePresent(string password)
    {
      var actual = _cut.IsUppercase(password);

      Assert.That(actual, Is.True);
    }

    [TestCase("abc123")]
    [TestCase("xep624")]
    [TestCase("pa5w0rd")]
    public void IsUppercase_ReturnsFalse_WhenUppercaseNotPresent(string password)
    {
      var actual = _cut.IsUppercase(password);

      Assert.That(actual, Is.False);
    }

    [TestCase("p@ssword")]
    [TestCase("P@SSWORD")]
    [TestCase("!!!!!")]
    public void IsSpecial_ReturnsTrue_WhenSpecialPresent(string password)
    {
      var actual = _cut.IsSpecial(password);

      Assert.That(actual, Is.True);
    }

    [TestCase("abc123")]
    [TestCase("XeP624")]
    [TestCase("465")]
    public void IsSpecial_ReturnsFalse_WhenSpecialNotPresent(string password)
    {
      var actual = _cut.IsSpecial(password);

      Assert.That(actual, Is.False);
    }
  }
}
