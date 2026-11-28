namespace HotelCRM.GUI.Navigation;

/// <summary>One sidebar entry. <paramref name="Path"/> is the Razor page name, e.g. "/frm201_dm_customer".</summary>
public sealed record MenuItem(string Path, string Title, string Group, string Icon)
{
    /// <summary>The three digits of the page code ("/frm201_dm_customer" gives "201").</summary>
    public string Code => Path.Substring(4, 3);
}
