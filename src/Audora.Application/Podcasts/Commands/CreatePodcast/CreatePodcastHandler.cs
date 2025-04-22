// using Audora.Domain.Entities;
// using MediatR;
//
// namespace Audora.Application.Podcasts.Commands.CreatePodcast;
//
// public class CreatePodcastHandler : IRequestHandler<CreatePodcastCommand, Podcast>
// {
//     private readonly IPodcastRepository _podcastRepository;
//     private readonly IUnitOfWork _unitOfWork;
//
//     public CreatePodcastHandler(IPodcastRepository podcastRepository, IUnitOfWork unitOfWork)
//     {
//         _podcastRepository = podcastRepository;
//         _unitOfWork = unitOfWork;
//     }
//
//     public async Task<Podcast> Handle(CreatePodcastCommand request, CancellationToken cancellationToken)
//     {
//         var podcast = new Podcast(
//             name: request.PodcastRequest.Name,
//             description: request.PodcastRequest.Description,
//             isPublished: request.PodcastRequest.IsPublished,
//             category: request.PodcastRequest.Category,
//             language: request.PodcastRequest.Language,
//             creatorId: request.PodcastRequest.CreatorId,
//             coverImageUrl: request.PodcastRequest.CoverImageUrl
//         );
//
//         await _podcastRepository.AddAsync(podcast);
//
//         foreach (var episode in request.PodcastRequest.Episodes ?? [])
//         {
//             var newEpisode = new Episode(
//                 podcastId: podcast.Id,
//                 audioFileId: episode.AudioFileId,
//                 name: episode.Name,
//                 description: episode.Description,
//                 coverImageUrl: episode.CoverImageUrl,
//                 isPublished: episode.IsPublished,
//                 podcastName: podcast.Name,
//                 episodeNumber: episode.EpisodeNumber
//             );
//
//             podcast.AddEpisode(newEpisode);
//         }
//
//         podcast.AddTags(request.PodcastRequest.Tags ?? []);
//
//         await _unitOfWork.CommitChangesAsync();
//
//         return podcast;
//     }
// }