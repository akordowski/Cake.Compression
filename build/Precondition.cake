public static class Precondition
{
    public static void IsNotNull(object obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException();
        }
    }

    public static void IsNotNull(object obj, string paramName)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(paramName);
        }
    }

    public static void IsNotNull(object obj, string paramName, string message)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(paramName, message);
        }
    }

    public static void IsNotNullOrWhiteSpace(string str)
    {
        if (str == null)
        {
            throw new ArgumentNullException();
        }
        else if (string.IsNullOrWhiteSpace(str))
        {
            string message = "Value cannot consists only of white-space characters.";
            throw new ArgumentException(message);
        }
    }

    public static void IsNotNullOrWhiteSpace(string str, string paramName)
    {
        if (str == null)
        {
            throw new ArgumentNullException(paramName);
        }
        else if (string.IsNullOrWhiteSpace(str))
        {
            string message = "Value cannot consists only of white-space characters.";
            throw new ArgumentException(message, paramName);
        }
    }

    public static void IsNotNullOrWhiteSpace(string str, string paramName, string message)
    {
        if (str == null)
        {
            throw new ArgumentNullException(paramName, message);
        }
        else if (string.IsNullOrWhiteSpace(str))
        {
            throw new ArgumentException(message, paramName);
        }
    }
}