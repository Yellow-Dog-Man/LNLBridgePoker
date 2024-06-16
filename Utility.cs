using System.Globalization;
using System.Net;

namespace LNLBridgePoker
{
    // From: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Net.Primitives/src/System/Net/IPEndPoint.cs
    // Older .nets don't have this so we hard code it here.
    public static class Utility
    {
        public static bool TryParse(string s, out IPEndPoint? result)
        {
            var success = TryParse(s.AsSpan(), out result);
            return success;
        }

        public static bool TryParse(ReadOnlySpan<char> s, out IPEndPoint? result)
        {
            int addressLength = s.Length;  // If there's no port then send the entire string to the address parser
            int lastColonPos = s.LastIndexOf(':');

            // Look to see if this is an IPv6 address with a port.
            if (lastColonPos > 0)
            {
                if (s[lastColonPos - 1] == ']')
                {
                    addressLength = lastColonPos;
                }
                // Look to see if this is IPv4 with a port (IPv6 will have another colon)
                else if (s.Slice(0, lastColonPos).LastIndexOf(':') == -1)
                {
                    addressLength = lastColonPos;
                }
            }

            if (IPAddress.TryParse(s.Slice(0, addressLength).ToString(), out IPAddress? address))
            {
                uint port = 0;
                if (addressLength == s.Length ||
                    (uint.TryParse(s.Slice(addressLength + 1).ToString(), NumberStyles.None, CultureInfo.InvariantCulture, out port) && port <= IPEndPoint.MaxPort))

                {
                    result = new IPEndPoint(address, (int)port);
                    return true;
                }
            }

            result = null;
            return false;
        }

        public static IPEndPoint? Parse(string s)
        {
            return Parse(s.AsSpan());
        }

        public static IPEndPoint? Parse(ReadOnlySpan<char> s)
        {
            if (TryParse(s, out IPEndPoint? result))
            {
                return result;
            }

            throw new FormatException("Invalid format");
        }
    }
}
