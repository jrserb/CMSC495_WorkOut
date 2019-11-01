using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WorkoutGen.Models
{
    public partial class WorkoutGenContext : DbContext
    {
        public WorkoutGenContext()
        {
        }

        public WorkoutGenContext(DbContextOptions<WorkoutGenContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<Exercise> Exercise { get; set; }
        public virtual DbSet<ExerciseAlternateEquipment> ExerciseAlternateEquipment { get; set; }
        public virtual DbSet<ExerciseEquipment> ExerciseEquipment { get; set; }
        public virtual DbSet<ExerciseMuscleGroup> ExerciseMuscleGroup { get; set; }
        public virtual DbSet<MuscleGroup> MuscleGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                    .HasConstraintName("FK__exercise___alter__59063A47");

                entity.HasOne(d => d.ExerciseEquipment)
                    .WithMany(p => p.ExerciseAlternateEquipment)
                    .HasForeignKey(d => d.ExerciseEquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__exercise___exerc__5812160E");
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
                    .HasConstraintName("FK__exercise___equip__5441852A");

                entity.HasOne(d => d.Exercise)
                    .WithMany(p => p.ExerciseEquipment)
                    .HasForeignKey(d => d.ExerciseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__exercise___exerc__534D60F1");
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
                    .HasConstraintName("FK__exercise___exerc__5CD6CB2B");

                entity.HasOne(d => d.MuscleGroup)
                    .WithMany(p => p.ExerciseMuscleGroup)
                    .HasForeignKey(d => d.MuscleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__exercise___muscl__5DCAEF64");
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
