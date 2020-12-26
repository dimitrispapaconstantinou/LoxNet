using System;
using System.Collections.Generic;
using System.Text;

namespace LoxNet.Scan
{
  public static class StringExtensions
  {
    /// <summary>
    /// Get the substring between the start and end indexes (like Java's substring function)
    /// </summary>
    /// <param name="s">The source string</param>
    /// <param name="start_index">The start index</param>
    /// <param name="end_index">The end index</param>
    /// <returns>The substring between start and end</returns>
    public static string Slice(this string s, int start, int end)
    {
      if (end < 0) // Keep this for negative end support
      {
        end = s.Length + end;
      }

      // Calculate the length of the substring
      int len = end - start;
      return s.Substring(start, len);
    }

  }
}
