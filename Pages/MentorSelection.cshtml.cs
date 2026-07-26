using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WebObrasci1.Models;
using WebObrasci1.Services;
using WebObrasci1.Settings;

namespace WebObrasci1.Pages
{
    [Authorize(Roles = Role.Student)]
    public class MentorSelectionModel : PageModel
    {
        private readonly IUserHelper _userHelper;
        private IOptions<UserSettings> _userSettings;
        public List<SelectListItem> MentoriSelectList { get; set; } = [];

        [BindProperty]
        public string? SelectedMentorJson { get; set; }

        public MentorSelectionModel(IUserHelper userHelper, IOptions<UserSettings> userSettings)
        {
            _userHelper = userHelper;
            _userSettings = userSettings;
        }

        public async Task OnGetAsync()
        {
            var user = await _userHelper.GetOrCreateUserAsync(User);
            var mentori = _userSettings.Value.Mentori ?? [];
            MentoriSelectList = mentori
                .Select(m => MentorToSelectListItem(m, user))
                .ToList();
        }

        private SelectListItem MentorToSelectListItem(MentorSettingsEntry mentor, User user)
        {
            var text = $"{mentor.Name} ({mentor.Email})";
            var isSelected = mentor.Name.Equals(user.MentorName) && mentor.Email.Equals(user.MentorMail);
            return new SelectListItem(text, JsonSerializer.Serialize(mentor), isSelected);
        }

        public async Task OnPostAsync()
        {
            var selectedMentor = SelectedMentorJson != null ? JsonSerializer.Deserialize<MentorSettingsEntry>(SelectedMentorJson) : null;
            await _userHelper.SaveMentorAsync(User, selectedMentor?.Name, selectedMentor?.Email);
            await OnGetAsync();
        }

    }
}
