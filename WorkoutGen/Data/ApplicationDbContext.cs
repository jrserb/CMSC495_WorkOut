using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkoutGen.Models;

namespace WorkoutGen.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<Exercise> Exercise { get; set; }
        public virtual DbSet<ExerciseAlternateEquipment> ExerciseAlternateEquipment { get; set; }
        public virtual DbSet<ExerciseEquipment> ExerciseEquipment { get; set; }
        public virtual DbSet<ExerciseMuscleGroup> ExerciseMuscleGroup { get; set; }
        public virtual DbSet<MuscleGroup> MuscleGroup { get; set; }
        public virtual DbSet<UserEquipmentSet> UserEquipmentSet { get; set; }
        public virtual DbSet<UserEquipmentSetEquipment> UserEquipmentSetEquipment { get; set; }
        public virtual DbSet<UserExercise> UserExercise { get; set; }
        public virtual DbSet<UserExerciseEquipment> UserExerciseEquipment { get; set; }
        public virtual DbSet<UserExerciseMuscleGroup> UserExerciseMuscleGroup { get; set; }
        public virtual DbSet<UserSet> UserSet { get; set; }
        public virtual DbSet<UserWorkout> UserWorkout { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  Explicitly call the OnModelCreating method of IdentityDbContext           
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("equipment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDeleted)
                    .HasColumnName("date_deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.ToTable("exercise");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDeleted)
                    .HasColumnName("date_deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .IsUnicode(false);

                entity.Property(e => e.Hyperlink)
                    .HasColumnName("hyperlink")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExerciseAlternateEquipment>(entity =>
            {
                entity.ToTable("exercise_alternate_equipment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AlternateEquipmentId).HasColumnName("alternate_equipment_id");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDeleted)
                    .HasColumnName("date_deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExerciseEquipmentId).HasColumnName("exercise_equipment_id");

                entity.HasOne(d => d.AlternateEquipment)
                    .WithMany(p => p.ExerciseAlternateEquipment)
                    .HasForeignKey(d => d.AlternateEquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__exercise___alter__25518C17");

                entity.HasOne(d => d.ExerciseEquipment)
                    .WithMany(p => p.ExerciseAlternateEquipment)
                    .HasForeignKey(d => d.ExerciseEquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__exercise___exerc__245D67DE");
            });

            modelBuilder.Entity<ExerciseEquipment>(entity =>
            {
                entity.ToTable("exercise_equipment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDeleted)
                    .HasColumnName("date_deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");

                entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.ExerciseEquipment)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__exercise___equip__208CD6FA");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.ExerciseEquipment)
                    .HasForeignKey(d => d.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__exercise___exerc__1F98B2C1");
            });

            modelBuilder.Entity<ExerciseMuscleGroup>(entity =>
            {
                entity.ToTable("exercise_muscle_group");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDeleted)
                    .HasColumnName("date_deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");

                entity.Property(e => e.MuscleGroupId).HasColumnName("muscle_group_id");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.ExerciseMuscleGroup)
                    .HasForeignKey(d => d.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__exercise___exerc__29221CFB");

                entity.HasOne(d => d.MuscleGroup)
                    .WithMany(p => p.ExerciseMuscleGroup)
                    .HasForeignKey(d => d.MuscleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__exercise___muscl__2A164134");
            });

            modelBuilder.Entity<MuscleGroup>(entity =>
            {
                entity.ToTable("muscle_group");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDeleted)
                    .HasColumnName("date_deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserEquipmentSet>(entity =>
            {
                entity.ToTable("user_equipment_set");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDeleted)
                    .HasColumnName("date_deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.Enabled).HasColumnName("enabled");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserEquipmentSet)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_equipment_set_AspNetUsers");
            });

            modelBuilder.Entity<UserEquipmentSetEquipment>(entity =>
            {
                entity.ToTable("user_equipment_set_equipment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDeleted)
                    .HasColumnName("date_deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");

                entity.Property(e => e.UserEquipmentSetId).HasColumnName("user_equipment_set_id");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.UserEquipmentSetEquipment)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_equipment_set_equipment_equipment1");
            });

            modelBuilder.Entity<UserExercise>(entity =>
            {
                entity.ToTable("user_exercise");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDeleted)
                    .HasColumnName("date_deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .IsUnicode(false);

                entity.Property(e => e.Hyperlink)
                    .HasColumnName("hyperlink")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserExercise)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_exercise_AspNetUsers");
            });

            modelBuilder.Entity<UserExerciseEquipment>(entity =>
            {
                entity.ToTable("user_exercise_equipment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDeleted)
                    .HasColumnName("date_deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");

                entity.Property(e => e.UserExerciseId).HasColumnName("user_exercise_id");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.UserExerciseEquipment)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_exercise_equipment_equipment");

                entity.HasOne(d => d.UserExercise)
                    .WithMany(p => p.UserExerciseEquipment)
                    .HasForeignKey(d => d.UserExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_exercise_equipment_user_exercise");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserExerciseEquipment)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_exercise_equipment_AspNetUsers");
            });

            modelBuilder.Entity<UserExerciseMuscleGroup>(entity =>
            {
                entity.ToTable("user_exercise_muscle_group");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDeleted)
                    .HasColumnName("date_deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.MuscleGroupId).HasColumnName("muscle_group_id");

                entity.Property(e => e.UserExerciseId).HasColumnName("user_exercise_id");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasMaxLength(450);

                entity.HasOne(d => d.MuscleGroup)
                    .WithMany(p => p.UserExerciseMuscleGroup)
                    .HasForeignKey(d => d.MuscleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_exercise_muscle_group_muscle_group");

                entity.HasOne(d => d.UserExercise)
                    .WithMany(p => p.UserExerciseMuscleGroup)
                    .HasForeignKey(d => d.UserExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_exercise_muscle_group_user_exercise");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserExerciseMuscleGroup)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_exercise_muscle_group_AspNetUsers");
            });

            modelBuilder.Entity<UserSet>(entity =>
            {
                entity.ToTable("user_set");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDeleted)
                    .HasColumnName("date_deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExerciseId).HasColumnName("exercise_id");

                entity.Property(e => e.Repetitions)
                    .HasColumnName("repetitions")
                    .HasMaxLength(50);

                entity.Property(e => e.UserExerciseId).HasColumnName("user_exercise_id");

                entity.Property(e => e.UserWorkoutId).HasColumnName("user_workout_id");

                entity.Property(e => e.Weight)
                    .HasColumnName("weight")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.UserSet)
                    .HasForeignKey(d => d.ExerciseId)
                    .HasConstraintName("FK_user_set_exercise");

                entity.HasOne(d => d.UserExercise)
                    .WithMany(p => p.UserSet)
                    .HasForeignKey(d => d.UserExerciseId)
                    .HasConstraintName("FK_user_set_user_exercise1");

                entity.HasOne(d => d.UserWorkout)
                    .WithMany(p => p.UserSet)
                    .HasForeignKey(d => d.UserWorkoutId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_user_set_user_workout");
            });

            modelBuilder.Entity<UserWorkout>(entity =>
            {
                entity.ToTable("user_workout");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateDeleted)
                    .HasColumnName("date_deleted")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserWorkout)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_user_workout_AspNetUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
