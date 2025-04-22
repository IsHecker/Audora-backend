IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'public')
BEGIN
    EXEC('CREATE SCHEMA [public]');
END;

CREATE TABLE [public].[Subscriptions] (
    [Id] uniqueidentifier NOT NULL,
    [ListenerId] uniqueidentifier NOT NULL,
    [PlanType] nvarchar NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    PRIMARY KEY ([Id], [ListenerId])
);

CREATE INDEX [public_Subscriptions_index_21]
ON [public].[Subscriptions] ([Id]);

CREATE INDEX [public_Subscriptions_index_22]
ON [public].[Subscriptions] ([ListenerId]);

CREATE INDEX [public_idx_Subscriptions_Id_UserId]
ON [public].[Subscriptions] ([Id], [ListenerId]);


CREATE TABLE [public].[ListeningProgress] (
    [Id] uniqueidentifier NOT NULL,
    [ListenerId] uniqueidentifier NOT NULL,
    [EpisodeId] uniqueidentifier NOT NULL,
    [LastPosition] bigint NOT NULL,
    [UpdatedAt] datetime2,
    PRIMARY KEY ([Id])
);


CREATE TABLE [public].[Profile] (
    [CreatorId] varchar NOT NULL,
    [Followers] bigint NOT NULL,
    [Bio] varchar NOT NULL,
    [Avatar] varchar NOT NULL,
    [DisplayName] varchar NOT NULL,
    [Website] varchar NOT NULL,
    PRIMARY KEY ([CreatorId])
);

CREATE INDEX [public_Profile_index_18]
ON [public].[Profile] ([CreatorId]);


CREATE TABLE [public].[Episodes] (
    [Id] uniqueidentifier NOT NULL,
    [PodcastId] varchar NOT NULL,
    [Title] varchar NOT NULL,
    [Description] varchar NOT NULL,
    [Duration] int NOT NULL,
    [AudioFileUrl] varchar NOT NULL,
    [PodcastTitle] bigint NOT NULL,
    [IsPublished] varchar NOT NULL,
    [EpisodeNumber] int NOT NULL,
    [Language] varchar NOT NULL,
    [Slug] varchar NOT NULL,
    [ReleaseDate] varchar NOT NULL,
    [CreatedAt] varchar NOT NULL,
    [UpdatedAt] varchar NOT NULL
);

CREATE INDEX [public_Episodes_index_4]
ON [public].[Episodes] ([Id]);

CREATE INDEX [public_Episodes_index_5]
ON [public].[Episodes] ([PodcastId]);

CREATE INDEX [public_idx_Episodes_Id_PodcastId]
ON [public].[Episodes] ([Id], [PodcastId]);


CREATE TABLE [public].[PodcastStats] (
    [PodcastId] varchar NOT NULL,
    [AverageRating] decimal NOT NULL,
    [RatingCount] int NOT NULL,
    PRIMARY KEY ([PodcastId])
);

CREATE INDEX [public_PodcastStats_index_12]
ON [public].[PodcastStats] ([PodcastId]);


CREATE TABLE [public].[EpisodeStats] (
    [Id] uniqueidentifier NOT NULL,
    [EpisodeId] varchar NOT NULL,
    [PlayCount] bigint NOT NULL,
    [TotalListenTime] int NOT NULL,
    [LikeCount] int NOT NULL,
    [Downloads] int NOT NULL,
    [Replays] varchar NOT NULL,
    [CompletionRate] decimal NOT NULL,
    [CommentsCount] int NOT NULL,
    [AvgListenDuration] decimal NOT NULL,
    [SkipCount] int NOT NULL,
    PRIMARY KEY ([Id])
);

CREATE INDEX [public_EpisodeStats_index_9]
ON [public].[EpisodeStats] ([EpisodeId]);


CREATE TABLE [public].[Reactions] (
    [Id] uniqueidentifier NOT NULL,
    [ListenerId] uniqueidentifier NOT NULL,
    [EntityId] uniqueidentifier NOT NULL,
    [EntityType] nvarchar NOT NULL,
    [ReactionType] tinyint NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    PRIMARY KEY ([Id])
);


CREATE TABLE [public].[Invitations] (
    [Id] varchar NOT NULL,
    [PodcastId] varchar NOT NULL,
    [InvitedEmail] varchar NOT NULL,
    [InvitedById] varchar NOT NULL,
    [CreatedAt] varchar NOT NULL,
    [AcceptedAt] varchar NOT NULL,
    [Status] varchar NOT NULL,
    [ExpiresAt] varchar NOT NULL
);

CREATE INDEX [public_Invitations_index_23]
ON [public].[Invitations] ([Id]);

CREATE INDEX [public_Invitations_index_24]
ON [public].[Invitations] ([PodcastId]);

CREATE INDEX [public_Invitations_index_25]
ON [public].[Invitations] ([InvitedById]);

CREATE INDEX [public_idx_Invitations_Id_PodcastId_InvitedById]
ON [public].[Invitations] ([Id], [PodcastId], [InvitedById]);


CREATE TABLE [public].[PodcastTags] (
    [TagId] varchar NOT NULL,
    [PodcastId] varchar NOT NULL
);

CREATE INDEX [public_PodcastTags_index_6]
ON [public].[PodcastTags] ([TagId]);

CREATE INDEX [public_PodcastTags_index_7]
ON [public].[PodcastTags] ([PodcastId]);

CREATE INDEX [public_idx_PodcastTags_TagId_PodcastId]
ON [public].[PodcastTags] ([TagId], [PodcastId]);


CREATE TABLE [public].[PlaylistEpisodes] (
    [PlayListId] uniqueidentifier NOT NULL,
    [EpisodeId] uniqueidentifier NOT NULL,
    [Order] int NOT NULL,
    [AddedAt] datetime2 NOT NULL,
    PRIMARY KEY ([PlayListId], [EpisodeId])
);


CREATE TABLE [public].[Analytics] (
    [Id] varchar NOT NULL,
    [CreatorId] varchar NOT NULL,
    [PlayCount] bigint NOT NULL
);

CREATE INDEX [public_Analytics_index_10]
ON [public].[Analytics] ([Id]);

CREATE INDEX [public_Analytics_index_11]
ON [public].[Analytics] ([CreatorId]);

CREATE INDEX [public_idx_Analytics_Id_CreatorId]
ON [public].[Analytics] ([Id], [CreatorId]);


CREATE TABLE [public].[Playlists] (
    [Id] uniqueidentifier NOT NULL,
    [ListenerId] uniqueidentifier NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [IsPublic] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [CoverImageUrl] nvarchar NOT NULL,
    PRIMARY KEY ([Id])
);


CREATE TABLE [public].[PlaybackHistory] (
    [Id] uniqueidentifier NOT NULL,
    [ListenerId] uniqueidentifier NOT NULL,
    [EpisodeId] uniqueidentifier NOT NULL,
    [PlayedAt] datetime2 NOT NULL,
    PRIMARY KEY ([Id])
);


CREATE TABLE [public].[Users] (
    [Id] uniqueidentifier NOT NULL,
    PRIMARY KEY ([Id])
);

CREATE INDEX [public_Users_index_3]
ON [public].[Users] ([Id]);


CREATE TABLE [public].[Notifications] (
    [Id] varchar NOT NULL,
    [UserId] varchar NOT NULL,
    [Message] varchar NOT NULL,
    [Type] varchar NOT NULL,
    [CreatedAt] varchar NOT NULL,
    [ReadStatus] varchar NOT NULL
);

CREATE INDEX [public_Notifications_index_15]
ON [public].[Notifications] ([Id]);

CREATE INDEX [public_Notifications_index_16]
ON [public].[Notifications] ([UserId]);

CREATE INDEX [public_idx_Notifications_Id_UserId]
ON [public].[Notifications] ([Id], [UserId]);


CREATE TABLE [public].[Tags] (
    [Id] varchar NOT NULL,
    [Name] varchar NOT NULL,
    PRIMARY KEY ([Id])
);

CREATE INDEX [public_Tags_index_8]
ON [public].[Tags] ([Id]);


CREATE TABLE [public].[Podcasts] (
    [Id] uniqueidentifier NOT NULL,
    [Title] varchar NOT NULL,
    [Description] text NOT NULL,
    [CoverImageUrl] text NOT NULL,
    [IsPublished] varchar NOT NULL,
    [CategoryId] uniqueidentifier NOT NULL,
    [Language] varchar NOT NULL,
    [TotalEpisodes] int NOT NULL,
    [Slug] varchar NOT NULL,
    [RssFeedUrl] text NOT NULL,
    [CreatorId] varchar NOT NULL,
    [CreatedAt] varchar NOT NULL,
    [UpdatedAt] varchar NOT NULL,
    PRIMARY KEY ([Id], [CategoryId])
);

CREATE INDEX [public_Podcasts_index_0]
ON [public].[Podcasts] ([Id]);

CREATE INDEX [public_Podcasts_index_1]
ON [public].[Podcasts] ([CategoryId]);

CREATE INDEX [public_Podcasts_index_2]
ON [public].[Podcasts] ([CreatorId]);

CREATE INDEX [public_idx_Podcasts_Id_CategoryId_CreatorId]
ON [public].[Podcasts] ([Id], [CategoryId], [CreatorId]);


CREATE TABLE [public].[Categories] (
    [Id] varchar NOT NULL,
    [Name] varchar NOT NULL,
    PRIMARY KEY ([Id])
);

CREATE INDEX [public_Categories_index_17]
ON [public].[Categories] ([Id]);


CREATE TABLE [public].[SocialLinks] (
    [Id] varchar NOT NULL,
    [CreatorId] varchar NOT NULL,
    [PlatformName] varchar NOT NULL,
    [Url] varchar NOT NULL
);

CREATE INDEX [public_SocialLinks_index_19]
ON [public].[SocialLinks] ([Id]);

CREATE INDEX [public_SocialLinks_index_20]
ON [public].[SocialLinks] ([CreatorId]);

CREATE INDEX [public_idx_SocialLinks_Id_CreatorId]
ON [public].[SocialLinks] ([Id], [CreatorId]);


/**
EntityType: Type of the entity being commented on (e.g., 'Episode', 'Podcast')
*/
CREATE TABLE [public].[Comments] (
    [Id] uniqueidentifier NOT NULL,
    [ListenerId] uniqueidentifier NOT NULL,
    [ParentId] uniqueidentifier NOT NULL,
    [EntityId] uniqueidentifier NOT NULL,
    [EntityType] varchar NOT NULL,
    [Content] nvarchar NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    PRIMARY KEY ([Id])
);

CREATE INDEX [public_Comments_index_13]
ON [public].[Comments] ([Id]);

CREATE INDEX [public_idx_Comments_Id_EpisodeId]
ON [public].[Comments] ([Id]);


CREATE TABLE [public].[PodcastRatings] (
    [PodcastId] uniqueidentifier NOT NULL,
    [ListenerId] uniqueidentifier NOT NULL,
    [Rating] tinyint NOT NULL,
    [AddedAt] datetime2 NOT NULL,
    PRIMARY KEY ([PodcastId], [ListenerId])
);


ALTER TABLE [public].[Analytics]
ADD CONSTRAINT [Analytics_CreatorId_Users_Id] FOREIGN KEY([CreatorId]) REFERENCES [public].[Users]([Id]);

ALTER TABLE [public].[Categories]
ADD CONSTRAINT [Categories_Id_Podcasts_CategoryId] FOREIGN KEY([Id]) REFERENCES [public].[Podcasts]([CategoryId]);

ALTER TABLE [public].[Comments]
ADD CONSTRAINT [Comments_Id_fk] FOREIGN KEY([Id]) REFERENCES [public].[Comments]([ParentId]);

ALTER TABLE [public].[Comments]
ADD CONSTRAINT [Comments_ListenerId_fk] FOREIGN KEY([ListenerId]) REFERENCES [public].[Users]([Id]);

ALTER TABLE [public].[Episodes]
ADD CONSTRAINT [Episodes_Id_EpisodeStats_EpisodeId] FOREIGN KEY([Id]) REFERENCES [public].[EpisodeStats]([EpisodeId]);

ALTER TABLE [public].[Invitations]
ADD CONSTRAINT [Invitations_InvitedById_Users_Id] FOREIGN KEY([InvitedById]) REFERENCES [public].[Users]([Id]);

ALTER TABLE [public].[Invitations]
ADD CONSTRAINT [Invitations_PodcastId_Podcasts_Id] FOREIGN KEY([PodcastId]) REFERENCES [public].[Podcasts]([Id]);

ALTER TABLE [public].[ListeningProgress]
ADD CONSTRAINT [ListeningProgress_EpisodeId_fk] FOREIGN KEY([EpisodeId]) REFERENCES [public].[Episodes]([Id]);

ALTER TABLE [public].[ListeningProgress]
ADD CONSTRAINT [ListeningProgress_ListenerId_fk] FOREIGN KEY([ListenerId]) REFERENCES [public].[Users]([Id]);

ALTER TABLE [public].[Notifications]
ADD CONSTRAINT [Notifications_UserId_Users_Id] FOREIGN KEY([UserId]) REFERENCES [public].[Users]([Id]);

ALTER TABLE [public].[PlaybackHistory]
ADD CONSTRAINT [PlaybackHistory_EpisodeId_fk] FOREIGN KEY([EpisodeId]) REFERENCES [public].[Episodes]([Id]);

ALTER TABLE [public].[PlaybackHistory]
ADD CONSTRAINT [PlaybackHistory_ListenerId_fk] FOREIGN KEY([ListenerId]) REFERENCES [public].[Users]([Id]);

ALTER TABLE [public].[PodcastRatings]
ADD CONSTRAINT [PodcastRatings_ListenerId_fk] FOREIGN KEY([ListenerId]) REFERENCES [public].[Users]([Id]);

ALTER TABLE [public].[PodcastRatings]
ADD CONSTRAINT [PodcastRatings_PodcastId_fk] FOREIGN KEY([PodcastId]) REFERENCES [public].[Podcasts]([Id]);

ALTER TABLE [public].[Podcasts]
ADD CONSTRAINT [Podcasts_CreatorId_Users_Id] FOREIGN KEY([CreatorId]) REFERENCES [public].[Users]([Id]);

ALTER TABLE [public].[Podcasts]
ADD CONSTRAINT [Podcasts_Id_Episodes_PodcastId] FOREIGN KEY([Id]) REFERENCES [public].[Episodes]([PodcastId]);

ALTER TABLE [public].[Podcasts]
ADD CONSTRAINT [Podcasts_Id_PodcastTags_PodcastId] FOREIGN KEY([Id]) REFERENCES [public].[PodcastTags]([PodcastId]);

ALTER TABLE [public].[PodcastStats]
ADD CONSTRAINT [PodcastStats_PodcastId_Podcasts_Id] FOREIGN KEY([PodcastId]) REFERENCES [public].[Podcasts]([Id]);

ALTER TABLE [public].[Profile]
ADD CONSTRAINT [Profile_CreatorId_Users_Id] FOREIGN KEY([CreatorId]) REFERENCES [public].[Users]([Id]);

ALTER TABLE [public].[SocialLinks]
ADD CONSTRAINT [SocialLinks_CreatorId_Profile_CreatorId] FOREIGN KEY([CreatorId]) REFERENCES [public].[Profile]([CreatorId]);

ALTER TABLE [public].[Subscriptions]
ADD CONSTRAINT [Subscriptions_UserId_Users_Id] FOREIGN KEY([ListenerId]) REFERENCES [public].[Users]([Id]);

ALTER TABLE [public].[Tags]
ADD CONSTRAINT [Tags_Id_PodcastTags_TagId] FOREIGN KEY([Id]) REFERENCES [public].[PodcastTags]([TagId]);

ALTER TABLE [public].[Comments]
ADD CONSTRAINT [Comments_EntityId_fk] FOREIGN KEY([EntityId]) REFERENCES [public].[Episodes]([Id]);

ALTER TABLE [public].[Playlists]
ADD CONSTRAINT [Playlists_ListenerId_fk] FOREIGN KEY([ListenerId]) REFERENCES [public].[Users]([Id]);

ALTER TABLE [public].[PlaylistEpisodes]
ADD CONSTRAINT [PlaylistEpisodes_PlayListId_fk] FOREIGN KEY([PlayListId]) REFERENCES [public].[Playlists]([Id]);

ALTER TABLE [public].[PlaylistEpisodes]
ADD CONSTRAINT [PlaylistEpisodes_EpisodeId_fk] FOREIGN KEY([EpisodeId]) REFERENCES [public].[Episodes]([Id]);

ALTER TABLE [public].[Reactions]
ADD CONSTRAINT [Reactions_ListenerId_fk] FOREIGN KEY([ListenerId]) REFERENCES [public].[Users]([Id]);

ALTER TABLE [public].[Reactions]
ADD CONSTRAINT [Reactions_EntityId_fk] FOREIGN KEY([EntityId]) REFERENCES [public].[EpisodeStats]([Id]);

ALTER TABLE [public].[Reactions]
ADD CONSTRAINT [Reactions_EntityId_fk] FOREIGN KEY([EntityId]) REFERENCES [public].[Comments]([Id]);
