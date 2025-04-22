using Audora.Contracts.Podcasts.Requests;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.Podcasts.Commands.CreatePodcast;

public record CreatePodcastCommand(CreatePodcastRequest PodcastRequest) : IRequest<Podcast>;