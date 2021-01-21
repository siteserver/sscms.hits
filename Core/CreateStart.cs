using System.Collections.Generic;
using System.Threading.Tasks;
using SSCMS.Configuration;
using SSCMS.Enums;
using SSCMS.Parse;
using SSCMS.Plugins;
using SSCMS.Repositories;
using SSCMS.Services;

namespace SSCMS.Hits.Core
{
    public class CreateStart : IPluginCreateStartAsync
    {
        private readonly IPathManager _pathManager;
        private readonly ISiteRepository _siteRepository;

        public CreateStart(IPathManager pathManager, ISiteRepository siteRepository)
        {
            _pathManager = pathManager;
            _siteRepository = siteRepository;
        }

        public async Task ParseAsync(IParseContext context)
        {
            if (context.TemplateType != TemplateType.ContentTemplate || context.ContentId <= 0) return;

            var site = await _siteRepository.GetAsync(context.SiteId);

            var apiUrl = _pathManager.GetApiHostUrl(site, Constants.ApiPrefix,
                $"/hits/{context.SiteId}/{context.ChannelId}/{context.ContentId}");

            context.FootCodes.TryAdd(HitsManager.PluginId, $@"<script src=""{apiUrl}"" type=""text/javascript""></script>");
        }
    }
}
