namespace HotelCRM.GUI.Navigation;

/// <summary>
/// The sidebar menu, one entry per page in the order of the BFD branches.
/// A new page needs only one line here; the layout draws groups and the active item from this list.
/// </summary>
public static class AppMenu
{
    private const string SystemGroup = "Hệ thống";
    private const string CareGroup = "Chăm sóc & hỗ trợ";
    private const string RelationsGroup = "Quan hệ khách hàng";

    public static IReadOnlyList<MenuItem> Items { get; } =
    [
        new("/frm001_main", "Trang chủ", "", "house"),
        new("/frm101_dm_staff", "Phòng ban & nhân viên", SystemGroup, "building"),
        new("/frm102_nv_ticket_create", "Lập phiếu yêu cầu", CareGroup, "plus-square"),
        new("/frm103_nv_ticket_search", "Tra cứu & phân công", CareGroup, "search"),
        new("/frm104_nv_ticket_process", "Xử lý phiếu yêu cầu", CareGroup, "check2-square"),
        new("/frm105_nv_survey", "Phiếu khảo sát", CareGroup, "clipboard-check"),
        new("/frm106_bc_customer_care", "Báo cáo CSKH", CareGroup, "bar-chart"),
        new("/frm201_dm_customer", "Hồ sơ khách 360", RelationsGroup, "person-vcard"),
        new("/frm202_dm_room", "Loại phòng & phòng", RelationsGroup, "door-closed"),
        new("/frm203_nv_stay", "Đặt phòng & lưu trú", RelationsGroup, "calendar-check"),
        new("/frm204_dm_reward", "Danh mục quà", RelationsGroup, "gift"),
        new("/frm205_nv_loyalty", "Sổ điểm & đổi quà", RelationsGroup, "star"),
        new("/frm206_nv_campaign", "Chương trình chăm sóc", RelationsGroup, "megaphone"),
        new("/frm207_bc_customer_360", "Báo cáo & dashboard 360", RelationsGroup, "graph-up"),
    ];

    /// <summary>The entry of the page being shown, or null for a page that is not in the menu.</summary>
    public static MenuItem? Find(string? path) => Items.FirstOrDefault(item => item.Path == path);
}
