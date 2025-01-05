using Microsoft.AspNetCore.Components;

namespace Portfolio.ToDo.Web.Components
{
    public partial class Pagination
    {
        [Parameter]
        public int TotalItems { get; set; }

        [Parameter]
        public int ItemsPerPage { get; set; } = 20;

        [Parameter]
        public int CurrentPage { get; set; } = 1;

        [Parameter]
        public EventCallback<int> OnPageChanged { get; set; }

        private int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);

        private async Task SetPage(int page)
        {
            if (page < 1 || page > TotalPages)
            {
                return;
            }

            CurrentPage = page;
            await OnPageChanged.InvokeAsync(page);
        }
    }
}
