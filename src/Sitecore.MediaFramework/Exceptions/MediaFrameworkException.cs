namespace Sitecore.MediaFramework.Exceptions
{
  using System;

  public class MediaFrameworkException : Exception
  {      
    /// <summary>
    /// Initializes a new instance of the <see cref="MediaFrameworkException"/> class.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <param name="ex">
    /// The ex.
    /// </param>
    public MediaFrameworkException(string message, Exception ex)
      : base(message, ex)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MediaFrameworkException"/> class.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    public MediaFrameworkException(string message)
      : base(message)
    {
    }
  }
}