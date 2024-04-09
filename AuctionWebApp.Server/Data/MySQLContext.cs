using AuctionWebApp.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionWebApp.Server.Data;

public partial class MySqlContext : DbContext, IDbContext
{
    public MySqlContext()
    {
    }

    public MySqlContext(DbContextOptions<MySqlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<AuctionComplaint> AuctionComplaints { get; set; }

    public virtual DbSet<AuctionComplaintParameter> AuctionComplaintParameters { get; set; }

    public virtual DbSet<AuctionComplaintReason> AuctionComplaintReasons { get; set; }

    public virtual DbSet<AuctionComplaintSolution> AuctionComplaintSolutions { get; set; }

    public virtual DbSet<AuctionType> AuctionTypes { get; set; }

    public virtual DbSet<Bid> Bids { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<CountryDelivery> CountryDeliveries { get; set; }

    public virtual DbSet<DailyStatistic> DailyStatistics { get; set; }

    public virtual DbSet<FinishedAuction> FinishedAuctions { get; set; }

    public virtual DbSet<InformationSection> InformationSections { get; set; }

    public virtual DbSet<ItemCondition> ItemConditions { get; set; }

    public virtual DbSet<Lot> Lots { get; set; }

    public virtual DbSet<LotAdditionalParameter> LotAdditionalParameters { get; set; }

    public virtual DbSet<LotCategory> LotCategories { get; set; }

    public virtual DbSet<MessageComplaint> MessageComplaints { get; set; }

    public virtual DbSet<MessageComplaintType> MessageComplaintTypes { get; set; }

    public virtual DbSet<PlatformNews> PlatformNews { get; set; }

    public virtual DbSet<PossibleSolution> PossibleSolutions { get; set; }

    public virtual DbSet<QuestionToAdministration> QuestionToAdministrations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SolutionParameter> SolutionParameters { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<TrackableLot> TrackableLots { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserReaction> UserReactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.AId).HasName("PRIMARY");

            entity.ToTable("answer");

            entity.HasIndex(e => new { e.AUserId, e.ATime }, "a_user_time_udx").IsUnique();

            entity.HasIndex(e => e.AQuestionId, "fk_a_question_idx");

            entity.HasIndex(e => e.AUserId, "fk_a_user_idx");

            entity.Property(e => e.AId)
                .ValueGeneratedNever()
                .HasColumnName("a_id");
            entity.Property(e => e.AQuestionId).HasColumnName("a_question_id");
            entity.Property(e => e.AText)
                .HasMaxLength(300)
                .HasColumnName("a_text")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ATime)
                .HasColumnType("datetime")
                .HasColumnName("a_time");
            entity.Property(e => e.AUserId).HasColumnName("a_user_id");

            entity.HasOne(d => d.AQuestion).WithMany(p => p.Answers)
                .HasForeignKey(d => d.AQuestionId)
                .HasConstraintName("fk_a_question");

            entity.HasOne(d => d.AUser).WithMany(p => p.Answers)
                .HasForeignKey(d => d.AUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_a_user");
        });

        modelBuilder.Entity<AuctionComplaint>(entity =>
        {
            entity.HasKey(e => e.AcId).HasName("PRIMARY");

            entity.ToTable("auction_complaint");

            entity.HasIndex(e => new { e.AcReasonId, e.AcDate, e.AcSolved, e.AcAuctionId }, "ac_reason_user_udx").IsUnique();

            entity.HasIndex(e => e.AcAuctionId, "fk_ac_auction_idx");

            entity.HasIndex(e => e.AcReasonId, "fk_ac_reason_idx");

            entity.Property(e => e.AcId)
                .ValueGeneratedNever()
                .HasColumnName("ac_id");
            entity.Property(e => e.AcAuctionId)
                .HasColumnType("bigint(9) unsigned zerofill")
                .HasColumnName("ac_auction_id");
            entity.Property(e => e.AcDate).HasColumnName("ac_date");
            entity.Property(e => e.AcDescription)
                .HasMaxLength(300)
                .HasDefaultValueSql("''")
                .HasColumnName("ac_description")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.AcReasonId).HasColumnName("ac_reason_id");
            entity.Property(e => e.AcSolved).HasColumnName("ac_solved");

            entity.HasOne(d => d.AcAuction).WithMany(p => p.AuctionComplaints)
                .HasForeignKey(d => d.AcAuctionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ac_auction");

            entity.HasOne(d => d.AcReason).WithMany(p => p.AuctionComplaints)
                .HasForeignKey(d => d.AcReasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ac_reason");
        });

        modelBuilder.Entity<AuctionComplaintParameter>(entity =>
        {
            entity.HasKey(e => new { e.AcpComplaintId, e.AcpName })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("auction_complaint_parameter");

            entity.Property(e => e.AcpComplaintId).HasColumnName("acp_complaint_id");
            entity.Property(e => e.AcpName)
                .HasMaxLength(100)
                .HasColumnName("acp_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.AcpExpression)
                .HasMaxLength(300)
                .HasDefaultValueSql("''")
                .HasColumnName("acp_expression")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.AcpComplaint).WithMany(p => p.AuctionComplaintParameters)
                .HasForeignKey(d => d.AcpComplaintId)
                .HasConstraintName("fk_acp_reason");
        });

        modelBuilder.Entity<AuctionComplaintReason>(entity =>
        {
            entity.HasKey(e => e.AcrId).HasName("PRIMARY");

            entity.ToTable("auction_complaint_reason");

            entity.Property(e => e.AcrId)
                .ValueGeneratedNever()
                .HasColumnName("acr_id");
            entity.Property(e => e.AcrName)
                .HasMaxLength(50)
                .HasColumnName("acr_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<AuctionComplaintSolution>(entity =>
        {
            entity.HasKey(e => e.AcsId).HasName("PRIMARY");

            entity.ToTable("auction_complaint_solution");

            entity.HasIndex(e => new { e.AcsComplaintId, e.AcsSolutionId, e.AcsDate }, "acs_complaint_solution_udx").IsUnique();

            entity.HasIndex(e => e.AcsAdminId, "fk_cs_admin_idx");

            entity.HasIndex(e => e.AcsComplaintId, "fk_cs_complaint_idx");

            entity.HasIndex(e => e.AcsSolutionId, "fk_cs_solution_idx");

            entity.Property(e => e.AcsId)
                .ValueGeneratedNever()
                .HasColumnName("acs_id");
            entity.Property(e => e.AcsAdminId).HasColumnName("acs_admin_id");
            entity.Property(e => e.AcsComplaintId).HasColumnName("acs_complaint_id");
            entity.Property(e => e.AcsDate).HasColumnName("acs_date");
            entity.Property(e => e.AcsSolutionId).HasColumnName("acs_solution_id");

            entity.HasOne(d => d.AcsAdmin).WithMany(p => p.AuctionComplaintSolutions)
                .HasForeignKey(d => d.AcsAdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cs_admin");

            entity.HasOne(d => d.AcsComplaint).WithMany(p => p.AuctionComplaintSolutions)
                .HasForeignKey(d => d.AcsComplaintId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cs_complaint");

            entity.HasOne(d => d.AcsSolution).WithMany(p => p.AuctionComplaintSolutions)
                .HasForeignKey(d => d.AcsSolutionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cs_solution");
        });

        modelBuilder.Entity<AuctionType>(entity =>
        {
            entity.HasKey(e => e.AtId).HasName("PRIMARY");

            entity.ToTable("auction_type");

            entity.Property(e => e.AtId).HasColumnName("at_id");
            entity.Property(e => e.AtName)
                .HasMaxLength(50)
                .HasColumnName("at_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Bid>(entity =>
        {
            entity.HasKey(e => e.BId1).HasName("PRIMARY");

            entity.ToTable("bid");

            entity.HasIndex(e => new { e.BLotId, e.BTime }, "b_lot_time_udx").IsUnique();

            entity.HasIndex(e => e.BLotId, "fk_b_lot_idx");

            entity.HasIndex(e => e.BParticipantId, "fk_b_participant_idx");

            entity.Property(e => e.BId1)
                .ValueGeneratedNever()
                .HasColumnName("b_id");
            entity.Property(e => e.BLotId)
                .HasColumnType("bigint(9) unsigned zerofill")
                .HasColumnName("b_lot_id");
            entity.Property(e => e.BParticipantId).HasColumnName("b_participant_id");
            entity.Property(e => e.BSize).HasColumnName("b_size");
            entity.Property(e => e.BTime)
                .HasMaxLength(6)
                .HasColumnName("b_time");

            entity.HasOne(d => d.BLot).WithMany(p => p.Bids)
                .HasForeignKey(d => d.BLotId)
                .HasConstraintName("fk_b_lot");

            entity.HasOne(d => d.BParticipant).WithMany(p => p.Bids)
                .HasForeignKey(d => d.BParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_b_participant");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PRIMARY");

            entity.ToTable("category");

            entity.HasIndex(e => e.CName, "c_name_udx").IsUnique();

            entity.Property(e => e.CId)
                .ValueGeneratedNever()
                .HasColumnName("c_id");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasColumnName("c_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CPaidPositionCoast).HasColumnName("c_paid_position_coast");
            entity.Property(e => e.СPaidPositionsCount).HasColumnName("с_paid_positions_count");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChId).HasName("PRIMARY");

            entity.ToTable("chat");

            entity.HasIndex(e => new { e.ChSellerId, e.ChUserId }, "ch_users_udx").IsUnique();

            entity.HasIndex(e => e.ChSellerId, "fk_ch_seller_idx");

            entity.HasIndex(e => e.ChUserId, "fk_ch_user_idx");

            entity.Property(e => e.ChId)
                .ValueGeneratedNever()
                .HasColumnName("ch_id");
            entity.Property(e => e.ChIsBlocked).HasColumnName("ch_is_blocked");
            entity.Property(e => e.ChSellerId).HasColumnName("ch_seller_id");
            entity.Property(e => e.ChUserId).HasColumnName("ch_user_id");

            entity.HasOne(d => d.ChSeller).WithMany(p => p.ChatChSellers)
                .HasForeignKey(d => d.ChSellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ch_seller");

            entity.HasOne(d => d.ChUser).WithMany(p => p.ChatChUsers)
                .HasForeignKey(d => d.ChUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ch_user");
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.CmId).HasName("PRIMARY");

            entity.ToTable("chat_message");

            entity.HasIndex(e => e.CmChatId, "fk_cm_chat_idx");

            entity.Property(e => e.CmId)
                .ValueGeneratedNever()
                .HasColumnName("cm_id");
            entity.Property(e => e.CmChatId).HasColumnName("cm_chat_id");
            entity.Property(e => e.CmText)
                .HasMaxLength(300)
                .HasColumnName("cm_text")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CmTime)
                .HasColumnType("datetime")
                .HasColumnName("cm_time");
            entity.Property(e => e.CmToSeller).HasColumnName("cm_to_seller");

            entity.HasOne(d => d.CmChat).WithMany(p => p.ChatMessages)
                .HasForeignKey(d => d.CmChatId)
                .HasConstraintName("fk_cm_chat");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.ComId).HasName("PRIMARY");

            entity.ToTable("comment");

            entity.HasIndex(e => new { e.ComUserId, e.ComTime }, "com_user_time_udx").IsUnique();

            entity.HasIndex(e => e.ComLotId, "fk_com_lot_idx");

            entity.HasIndex(e => e.ComUserId, "fk_com_user_idx");

            entity.Property(e => e.ComId).HasColumnName("com_id");
            entity.Property(e => e.ComLotId)
                .HasColumnType("bigint(9) unsigned zerofill")
                .HasColumnName("com_lot_id");
            entity.Property(e => e.ComText)
                .HasMaxLength(500)
                .HasColumnName("com_text")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ComTime)
                .HasColumnType("datetime")
                .HasColumnName("com_time");
            entity.Property(e => e.ComUserId).HasColumnName("com_user_id");

            entity.HasOne(d => d.ComLot).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ComLotId)
                .HasConstraintName("fk_com_lot");

            entity.HasOne(d => d.ComUser).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ComUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_com_user");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CouId).HasName("PRIMARY");

            entity.ToTable("country");

            entity.Property(e => e.CouId)
                .ValueGeneratedNever()
                .HasColumnName("cou_id");
            entity.Property(e => e.CouName)
                .HasMaxLength(50)
                .HasColumnName("cou_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<CountryDelivery>(entity =>
        {
            entity.HasKey(e => new { e.CdLotId, e.CdCountryId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("country_delivery");

            entity.HasIndex(e => e.CdCountryId, "fk_cd_country_idx");

            entity.HasIndex(e => e.CdLotId, "fk_cd_lot_idx");

            entity.Property(e => e.CdLotId)
                .HasColumnType("bigint(9) unsigned zerofill")
                .HasColumnName("cd_lot_id");
            entity.Property(e => e.CdCountryId).HasColumnName("cd_country_id");
            entity.Property(e => e.CdSize).HasColumnName("cd_size");

            entity.HasOne(d => d.CdCountry).WithMany(p => p.CountryDeliveries)
                .HasForeignKey(d => d.CdCountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cd_country");

            entity.HasOne(d => d.CdLot).WithMany(p => p.CountryDeliveries)
                .HasForeignKey(d => d.CdLotId)
                .HasConstraintName("fk_cd_lot");
        });

        modelBuilder.Entity<DailyStatistic>(entity =>
        {
            entity.HasKey(e => e.DsDate).HasName("PRIMARY");

            entity.ToTable("daily_statistic");

            entity.Property(e => e.DsDate).HasColumnName("ds_date");
            entity.Property(e => e.DsBidsCount).HasColumnName("ds_bids_count");
            entity.Property(e => e.DsCompaintsCount).HasColumnName("ds_compaints_count");
            entity.Property(e => e.DsNewLotsCount).HasColumnName("ds_new_lots_count");
            entity.Property(e => e.DsQuestionsCount).HasColumnName("ds_questions_count");
            entity.Property(e => e.DsRegistrationsCount).HasColumnName("ds_registrations_count");
            entity.Property(e => e.DsVisitsCount).HasColumnName("ds_visits_count");
        });

        modelBuilder.Entity<FinishedAuction>(entity =>
        {
            entity.HasKey(e => e.FaId).HasName("PRIMARY");

            entity.ToTable("finished_auction");

            entity.HasIndex(e => new { e.FaSellerId, e.FaName, e.FaFinishTime }, "fa_seller_name_time_udx").IsUnique();

            entity.HasIndex(e => e.FaAuctionType, "fk_fa_auction_type_idx");

            entity.HasIndex(e => e.FaConditionId, "fk_fa_condition_idx");

            entity.HasIndex(e => e.FaCurrentStateId, "fk_fa_current_state_idx");

            entity.HasIndex(e => e.FaSellerId, "fk_fa_seller_idx");

            entity.HasIndex(e => e.FaWinnerId, "fk_fa_winner_idx");

            entity.Property(e => e.FaId)
                .HasColumnType("bigint(9) unsigned zerofill")
                .HasColumnName("fa_id");
            entity.Property(e => e.FaAuctionType).HasColumnName("fa_auction_type");
            entity.Property(e => e.FaConditionId).HasColumnName("fa_condition_id");
            entity.Property(e => e.FaCurrentStateId).HasColumnName("fa_current_state_id");
            entity.Property(e => e.FaDeliveryAddress)
                .HasMaxLength(500)
                .HasColumnName("fa_delivery_address")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.FaDescription)
                .HasMaxLength(1000)
                .HasDefaultValueSql("''")
                .HasColumnName("fa_description")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.FaFinishTime)
                .HasColumnType("datetime")
                .HasColumnName("fa_finish_time");
            entity.Property(e => e.FaName)
                .HasMaxLength(50)
                .HasColumnName("fa_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.FaSellerId).HasColumnName("fa_seller_id");
            entity.Property(e => e.FaSize).HasColumnName("fa_size");
            entity.Property(e => e.FaStateUpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("fa_state_update_time");
            entity.Property(e => e.FaTracingCode)
                .HasMaxLength(100)
                .HasColumnName("fa_tracing_code")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.FaWinnerId).HasColumnName("fa_winner_id");

            entity.HasOne(d => d.FaAuctionTypeNavigation).WithMany(p => p.FinishedAuctions)
                .HasForeignKey(d => d.FaAuctionType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fa_auction_type");

            entity.HasOne(d => d.FaCondition).WithMany(p => p.FinishedAuctions)
                .HasForeignKey(d => d.FaConditionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fa_condition");

            entity.HasOne(d => d.FaCurrentState).WithMany(p => p.FinishedAuctions)
                .HasForeignKey(d => d.FaCurrentStateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fa_current_state");

            entity.HasOne(d => d.FaSeller).WithMany(p => p.FinishedAuctionFaSellers)
                .HasForeignKey(d => d.FaSellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fa_seller");

            entity.HasOne(d => d.FaWinner).WithMany(p => p.FinishedAuctionFaWinners)
                .HasForeignKey(d => d.FaWinnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_fa_winner");
        });

        modelBuilder.Entity<InformationSection>(entity =>
        {
            entity.HasKey(e => e.IsId).HasName("PRIMARY");

            entity.ToTable("information_section");

            entity.HasIndex(e => e.IsParentSectionId, "fk_is_parent_section_idx");

            entity.HasIndex(e => new { e.IsName, e.IsParentSectionId }, "is_section_name_udx").IsUnique();

            entity.Property(e => e.IsId).HasColumnName("is_id");
            entity.Property(e => e.IsName)
                .HasMaxLength(100)
                .HasColumnName("is_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.IsParentSectionId).HasColumnName("is_parent_section_id");
            entity.Property(e => e.IsText)
                .HasColumnType("mediumtext")
                .HasColumnName("is_text");

            entity.HasOne(d => d.IsParentSection).WithMany(p => p.InverseIsParentSection)
                .HasForeignKey(d => d.IsParentSectionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_is_parent_section");
        });

        modelBuilder.Entity<ItemCondition>(entity =>
        {
            entity.HasKey(e => e.IcId).HasName("PRIMARY");

            entity.ToTable("item_condition");

            entity.Property(e => e.IcId).HasColumnName("ic_id");
            entity.Property(e => e.IcName)
                .HasMaxLength(50)
                .HasColumnName("ic_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Lot>(entity =>
        {
            entity.HasKey(e => e.LId).HasName("PRIMARY");

            entity.ToTable("lot");

            entity.HasIndex(e => e.LAuctionType, "fk_l_auction_type_idx");

            entity.HasIndex(e => e.LConditionId, "fk_l_item_condition_idx");

            entity.HasIndex(e => e.LSellerId, "fk_l_seller_idx");

            entity.HasIndex(e => e.LFinishTime, "l_finish");

            entity.HasIndex(e => new { e.LSellerId, e.LName }, "l_seller_lot_udx").IsUnique();

            entity.Property(e => e.LId)
                .HasColumnType("bigint(9) unsigned zerofill")
                .HasColumnName("l_id");
            entity.Property(e => e.LAuctionType).HasColumnName("l_auction_type");
            entity.Property(e => e.LConditionId).HasColumnName("l_condition_id");
            entity.Property(e => e.LCostStep).HasColumnName("l_cost_step");
            entity.Property(e => e.LDescription)
                .HasMaxLength(500)
                .HasDefaultValueSql("''")
                .HasColumnName("l_description")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LFinishTime)
                .HasColumnType("datetime")
                .HasColumnName("l_finish_time");
            entity.Property(e => e.LInitialCost).HasColumnName("l_initial_cost");
            entity.Property(e => e.LName)
                .HasMaxLength(50)
                .HasColumnName("l_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LRatingRestriction).HasColumnName("l_rating_restriction");
            entity.Property(e => e.LSellerId).HasColumnName("l_seller_id");
            entity.Property(e => e.LStartTime)
                .HasColumnType("datetime")
                .HasColumnName("l_start_time");

            entity.HasOne(d => d.LAuctionTypeNavigation).WithMany(p => p.Lots)
                .HasForeignKey(d => d.LAuctionType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_l_auction_type");

            entity.HasOne(d => d.LCondition).WithMany(p => p.Lots)
                .HasForeignKey(d => d.LConditionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_l_item_condition");

            entity.HasOne(d => d.LSeller).WithMany(p => p.Lots)
                .HasForeignKey(d => d.LSellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_l_seller");
        });

        modelBuilder.Entity<LotAdditionalParameter>(entity =>
        {
            entity.HasKey(e => new { e.LapName, e.LapLotId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("lot_additional_parameter");

            entity.HasIndex(e => e.LapLotId, "fk_lap_lot_category_idx");

            entity.Property(e => e.LapName)
                .HasMaxLength(50)
                .HasColumnName("lap_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LapLotId)
                .HasColumnType("bigint(9) unsigned zerofill")
                .HasColumnName("lap_lot_id");
            entity.Property(e => e.LapValue)
                .HasMaxLength(150)
                .HasColumnName("lap_value")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.LapLot).WithMany(p => p.LotAdditionalParameters)
                .HasForeignKey(d => d.LapLotId)
                .HasConstraintName("fk_lap_lot");
        });

        modelBuilder.Entity<LotCategory>(entity =>
        {
            entity.HasKey(e => new { e.LcLotId, e.LcCategoryId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("lot_category");

            entity.HasIndex(e => e.LcCategoryId, "fk_lc_category_idx");

            entity.HasIndex(e => e.LcLotId, "fk_lc_lot_idx");

            entity.Property(e => e.LcLotId)
                .HasColumnType("bigint(9) unsigned zerofill")
                .HasColumnName("lc_lot_id");
            entity.Property(e => e.LcCategoryId).HasColumnName("lc_category_id");
            entity.Property(e => e.LcPremiumEnd).HasColumnName("lc_premium_end");
            entity.Property(e => e.LcPremiumStart).HasColumnName("lc_premium_start");

            entity.HasOne(d => d.LcCategory).WithMany(p => p.LotCategories)
                .HasForeignKey(d => d.LcCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_lc_category");

            entity.HasOne(d => d.LcLot).WithMany(p => p.LotCategories)
                .HasForeignKey(d => d.LcLotId)
                .HasConstraintName("fk_lc_lot");
        });

        modelBuilder.Entity<MessageComplaint>(entity =>
        {
            entity.HasKey(e => e.McId).HasName("PRIMARY");

            entity.ToTable("message_complaint");

            entity.HasIndex(e => e.McTypeId, "fk_mc_type_idx");

            entity.HasIndex(e => e.McUserId, "fk_mc_user_idx");

            entity.Property(e => e.McId)
                .ValueGeneratedNever()
                .HasColumnName("mc_id");
            entity.Property(e => e.McConfirmed).HasColumnName("mc_confirmed");
            entity.Property(e => e.McDate).HasColumnName("mc_date");
            entity.Property(e => e.McTypeId).HasColumnName("mc_type_id");
            entity.Property(e => e.McUserId).HasColumnName("mc_user_id");
            entity.Property(e => e.MsMessageId).HasColumnName("ms_message_id");

            entity.HasOne(d => d.McType).WithMany(p => p.MessageComplaints)
                .HasForeignKey(d => d.McTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_mc_type");

            entity.HasOne(d => d.McUser).WithMany(p => p.MessageComplaints)
                .HasForeignKey(d => d.McUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_mc_user");
        });

        modelBuilder.Entity<MessageComplaintType>(entity =>
        {
            entity.HasKey(e => e.MctId).HasName("PRIMARY");

            entity.ToTable("message_complaint_type");

            entity.HasIndex(e => e.MctName, "mct_name_udx").IsUnique();

            entity.Property(e => e.MctId).HasColumnName("mct_id");
            entity.Property(e => e.MctName)
                .HasMaxLength(50)
                .HasColumnName("mct_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.MctRatingPanishment).HasColumnName("mct_rating_panishment");
        });

        modelBuilder.Entity<PlatformNews>(entity =>
        {
            entity.HasKey(e => e.PnId).HasName("PRIMARY");

            entity.ToTable("platform_news");

            entity.HasIndex(e => new { e.PnTitle, e.PnPublicationTime }, "pn_title_time_udx").IsUnique();

            entity.Property(e => e.PnId).HasColumnName("pn_id");
            entity.Property(e => e.PnPublicationTime)
                .HasColumnType("datetime")
                .HasColumnName("pn_publication_time");
            entity.Property(e => e.PnText)
                .HasColumnType("text")
                .HasColumnName("pn_text");
            entity.Property(e => e.PnTitle)
                .HasMaxLength(100)
                .HasColumnName("pn_title")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<PossibleSolution>(entity =>
        {
            entity.HasKey(e => e.PsId).HasName("PRIMARY");

            entity.ToTable("possible_solution");

            entity.HasIndex(e => e.PsName, "ps_name_udx").IsUnique();

            entity.Property(e => e.PsId)
                .ValueGeneratedNever()
                .HasColumnName("ps_id");
            entity.Property(e => e.PsName)
                .HasMaxLength(50)
                .HasColumnName("ps_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PsSellerNotificationTemplate)
                .HasMaxLength(500)
                .HasDefaultValueSql("''")
                .HasColumnName("ps_seller_notification_template")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PsUserNotificationTemplate)
                .HasMaxLength(500)
                .HasDefaultValueSql("''")
                .HasColumnName("ps_user_notification_template")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<QuestionToAdministration>(entity =>
        {
            entity.HasKey(e => e.QtaId).HasName("PRIMARY");

            entity.ToTable("question_to_administration");

            entity.HasIndex(e => e.QtaUserId, "fk_qta_user_idx");

            entity.HasIndex(e => new { e.QtaQuestion, e.QtaSolved }, "qta_question_solution_udx").IsUnique();

            entity.Property(e => e.QtaId).HasColumnName("qta_id");
            entity.Property(e => e.QtaDate)
                .HasColumnType("datetime")
                .HasColumnName("qta_date");
            entity.Property(e => e.QtaQuestion)
                .HasMaxLength(300)
                .HasColumnName("qta_question")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.QtaSolved).HasColumnName("qta_solved");
            entity.Property(e => e.QtaUserId).HasColumnName("qta_user_id");
            entity.Property(e => e.QuaRating).HasColumnName("qua_rating");

            entity.HasOne(d => d.QtaUser).WithMany(p => p.QuestionToAdministrations)
                .HasForeignKey(d => d.QtaUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_qta_user");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.RId).HasColumnName("r_id");
            entity.Property(e => e.RName)
                .HasMaxLength(50)
                .HasColumnName("r_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<SolutionParameter>(entity =>
        {
            entity.HasKey(e => new { e.SpName, e.SpSolutionId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("solution_parameter");

            entity.HasIndex(e => e.SpSolutionId, "fk_sp_solution_idx");

            entity.Property(e => e.SpName)
                .HasMaxLength(50)
                .HasColumnName("sp_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SpSolutionId).HasColumnName("sp_solution_id");
            entity.Property(e => e.SpValue)
                .HasMaxLength(150)
                .HasColumnName("sp_value")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.SpSolution).WithMany(p => p.SolutionParameters)
                .HasForeignKey(d => d.SpSolutionId)
                .HasConstraintName("fk_solution");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.SId).HasName("PRIMARY");

            entity.ToTable("state");

            entity.Property(e => e.SId).HasColumnName("s_id");
            entity.Property(e => e.SName)
                .HasMaxLength(50)
                .HasColumnName("s_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<TrackableLot>(entity =>
        {
            entity.HasKey(e => new { e.TlUserId, e.TlLotId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("trackable_lot");

            entity.HasIndex(e => e.TlLotId, "fk_tl_lot_idx");

            entity.HasIndex(e => e.TlUserId, "fk_tl_user_idx");

            entity.Property(e => e.TlUserId).HasColumnName("tl_user_id");
            entity.Property(e => e.TlLotId)
                .HasColumnType("bigint(9) unsigned zerofill")
                .HasColumnName("tl_lot_id");
            entity.Property(e => e.TlMaxAutomaticBid).HasColumnName("tl_max_automatic_bid");

            entity.HasOne(d => d.TlLot).WithMany(p => p.TrackableLots)
                .HasForeignKey(d => d.TlLotId)
                .HasConstraintName("fk_tl_lot");

            entity.HasOne(d => d.TlUser).WithMany(p => p.TrackableLots)
                .HasForeignKey(d => d.TlUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tl_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.UCountryId, "fk_u_country_idx");

            entity.HasIndex(e => e.URoleId, "fk_u_role_idx");

            entity.HasIndex(e => e.UEmail, "u_email_udx").IsUnique();

            entity.HasIndex(e => e.UName, "u_name_udx").IsUnique();

            entity.Property(e => e.UId).HasColumnName("u_id");
            entity.Property(e => e.UAddress)
                .HasMaxLength(300)
                .HasDefaultValueSql("''")
                .HasColumnName("u_address")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UBalance).HasColumnName("u_balance");
            entity.Property(e => e.UCountryId).HasColumnName("u_country_id");
            entity.Property(e => e.UEmail)
                .HasMaxLength(100)
                .HasColumnName("u_email");
            entity.Property(e => e.UName)
                .HasMaxLength(50)
                .HasColumnName("u_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UPasswordHash)
                .HasMaxLength(256)
                .HasColumnName("u_password_hash")
                .UseCollation("utf8mb4_bin");
            entity.Property(e => e.URating)
                .HasDefaultValueSql("'10'")
                .HasColumnName("u_rating");
            entity.Property(e => e.URegistrationDate).HasColumnName("u_registration_date");
            entity.Property(e => e.URoleId).HasColumnName("u_role_id");

            entity.HasOne(d => d.UCountry).WithMany(p => p.Users)
                .HasForeignKey(d => d.UCountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_u_country");

            entity.HasOne(d => d.URole).WithMany(p => p.Users)
                .HasForeignKey(d => d.URoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_u_role");
        });

        modelBuilder.Entity<UserReaction>(entity =>
        {
            entity.HasKey(e => new { e.UrUserId, e.UrAnswerId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("user_reaction");

            entity.HasIndex(e => e.UrAnswerId, "fk_ur_answer_idx");

            entity.Property(e => e.UrUserId).HasColumnName("ur_user_id");
            entity.Property(e => e.UrAnswerId).HasColumnName("ur_answer_id");
            entity.Property(e => e.UrIsPositive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("ur_is_positive");

            entity.HasOne(d => d.UrAnswer).WithMany(p => p.UserReactions)
                .HasForeignKey(d => d.UrAnswerId)
                .HasConstraintName("fk_ur_answer");

            entity.HasOne(d => d.UrUser).WithMany(p => p.UserReactions)
                .HasForeignKey(d => d.UrUserId)
                .HasConstraintName("fk_ur_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
