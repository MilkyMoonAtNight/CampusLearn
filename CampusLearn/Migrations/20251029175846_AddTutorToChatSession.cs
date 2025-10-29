using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CampusLearn.Migrations
{
    /// <inheritdoc />
    public partial class AddTutorToChatSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "faculty",
                columns: table => new
                {
                    facultyid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    facultyname = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faculty", x => x.facultyid);
                });

            migrationBuilder.CreateTable(
                name: "forumtopic",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false),
                    contributions = table.Column<int>(type: "integer", nullable: false),
                    progress = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forumtopic", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MessageUsers",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Groups = table.Column<List<string>>(type: "text[]", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageUsers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "modulecluster",
                columns: table => new
                {
                    clusterid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    clustername = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modulecluster", x => x.clusterid);
                });

            migrationBuilder.CreateTable(
                name: "rating",
                columns: table => new
                {
                    ratingid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ratingvalue = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rating", x => x.ratingid);
                });

            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    sessionid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sessiontopic = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session", x => x.sessionid);
                });

            migrationBuilder.CreateTable(
                name: "speciality",
                columns: table => new
                {
                    specialityid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    specialityname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_speciality", x => x.specialityid);
                });

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    studentid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    personalemail = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    passwordhash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student", x => x.studentid);
                });

            migrationBuilder.CreateTable(
                name: "degree",
                columns: table => new
                {
                    degreeid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    degreename = table.Column<string>(type: "text", nullable: false),
                    facultyid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_degree", x => x.degreeid);
                    table.ForeignKey(
                        name: "FK_degree_faculty_facultyid",
                        column: x => x.facultyid,
                        principalTable: "faculty",
                        principalColumn: "facultyid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reply",
                columns: table => new
                {
                    replyid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    forumtopicid = table.Column<int>(type: "integer", nullable: false),
                    author = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    postedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reply", x => x.replyid);
                    table.ForeignKey(
                        name: "FK_reply_forumtopic_forumtopicid",
                        column: x => x.forumtopicid,
                        principalTable: "forumtopic",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessionrating",
                columns: table => new
                {
                    sessionid = table.Column<long>(type: "bigint", nullable: false),
                    ratingid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessionrating", x => new { x.sessionid, x.ratingid });
                    table.ForeignKey(
                        name: "FK_sessionrating_rating_ratingid",
                        column: x => x.ratingid,
                        principalTable: "rating",
                        principalColumn: "ratingid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sessionrating_session_sessionid",
                        column: x => x.sessionid,
                        principalTable: "session",
                        principalColumn: "sessionid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tutors",
                columns: table => new
                {
                    tutorid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tutorname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    tutorsurname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    specialityid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tutors", x => x.tutorid);
                    table.ForeignKey(
                        name: "FK_tutors_speciality_specialityid",
                        column: x => x.specialityid,
                        principalTable: "speciality",
                        principalColumn: "specialityid");
                });

            migrationBuilder.CreateTable(
                name: "chatsession",
                columns: table => new
                {
                    chatsessionid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    studentid = table.Column<long>(type: "bigint", nullable: false),
                    startedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    endedat = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    topic = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatsession", x => x.chatsessionid);
                    table.ForeignKey(
                        name: "FK_chatsession_student_studentid",
                        column: x => x.studentid,
                        principalTable: "student",
                        principalColumn: "studentid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "enrollment",
                columns: table => new
                {
                    enrollmentid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    studentid = table.Column<long>(type: "bigint", nullable: false),
                    enrollmentdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enrollment", x => x.enrollmentid);
                    table.ForeignKey(
                        name: "FK_enrollment_student_studentid",
                        column: x => x.studentid,
                        principalTable: "student",
                        principalColumn: "studentid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessionstudent",
                columns: table => new
                {
                    sessionid = table.Column<long>(type: "bigint", nullable: false),
                    studentid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessionstudent", x => new { x.sessionid, x.studentid });
                    table.ForeignKey(
                        name: "FK_sessionstudent_session_sessionid",
                        column: x => x.sessionid,
                        principalTable: "session",
                        principalColumn: "sessionid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sessionstudent_student_studentid",
                        column: x => x.studentid,
                        principalTable: "student",
                        principalColumn: "studentid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessiontutor",
                columns: table => new
                {
                    sessionid = table.Column<long>(type: "bigint", nullable: false),
                    tutorid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessiontutor", x => new { x.sessionid, x.tutorid });
                    table.ForeignKey(
                        name: "FK_sessiontutor_session_sessionid",
                        column: x => x.sessionid,
                        principalTable: "session",
                        principalColumn: "sessionid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sessiontutor_tutors_tutorid",
                        column: x => x.tutorid,
                        principalTable: "tutors",
                        principalColumn: "tutorid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "studenttutor",
                columns: table => new
                {
                    studentid = table.Column<long>(type: "bigint", nullable: false),
                    tutorid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studenttutor", x => new { x.studentid, x.tutorid });
                    table.ForeignKey(
                        name: "FK_studenttutor_student_studentid",
                        column: x => x.studentid,
                        principalTable: "student",
                        principalColumn: "studentid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_studenttutor_tutors_tutorid",
                        column: x => x.tutorid,
                        principalTable: "tutors",
                        principalColumn: "tutorid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "topicmodule",
                columns: table => new
                {
                    moduleid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    modulename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    clusterid = table.Column<int>(type: "integer", nullable: true),
                    moduleheadid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topicmodule", x => x.moduleid);
                    table.ForeignKey(
                        name: "FK_topicmodule_modulecluster_clusterid",
                        column: x => x.clusterid,
                        principalTable: "modulecluster",
                        principalColumn: "clusterid");
                    table.ForeignKey(
                        name: "FK_topicmodule_tutors_moduleheadid",
                        column: x => x.moduleheadid,
                        principalTable: "tutors",
                        principalColumn: "tutorid",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "chatmessages",
                columns: table => new
                {
                    chatmessageid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    chatsessionid = table.Column<long>(type: "bigint", nullable: false),
                    isfromstudent = table.Column<bool>(type: "boolean", nullable: false),
                    messagetext = table.Column<string>(type: "text", nullable: false),
                    sentat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatmessages", x => x.chatmessageid);
                    table.ForeignKey(
                        name: "FK_chatmessages_chatsession_chatsessionid",
                        column: x => x.chatsessionid,
                        principalTable: "chatsession",
                        principalColumn: "chatsessionid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "enrollmentdegree",
                columns: table => new
                {
                    enrollmentid = table.Column<long>(type: "bigint", nullable: false),
                    degreeid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enrollmentdegree", x => new { x.enrollmentid, x.degreeid });
                    table.ForeignKey(
                        name: "FK_enrollmentdegree_degree_degreeid",
                        column: x => x.degreeid,
                        principalTable: "degree",
                        principalColumn: "degreeid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_enrollmentdegree_enrollment_enrollmentid",
                        column: x => x.enrollmentid,
                        principalTable: "enrollment",
                        principalColumn: "enrollmentid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "degreemodule",
                columns: table => new
                {
                    degreeid = table.Column<int>(type: "integer", nullable: false),
                    moduleid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_degreemodule", x => new { x.degreeid, x.moduleid });
                    table.ForeignKey(
                        name: "FK_degreemodule_degree_degreeid",
                        column: x => x.degreeid,
                        principalTable: "degree",
                        principalColumn: "degreeid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_degreemodule_topicmodule_moduleid",
                        column: x => x.moduleid,
                        principalTable: "topicmodule",
                        principalColumn: "moduleid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "moduleresource",
                columns: table => new
                {
                    resourceid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    moduleid = table.Column<int>(type: "integer", nullable: false),
                    resourcetype = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    resourceurl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moduleresource", x => x.resourceid);
                    table.ForeignKey(
                        name: "FK_moduleresource_topicmodule_moduleid",
                        column: x => x.moduleid,
                        principalTable: "topicmodule",
                        principalColumn: "moduleid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chatmessages_chatsessionid",
                table: "chatmessages",
                column: "chatsessionid");

            migrationBuilder.CreateIndex(
                name: "IX_chatsession_studentid",
                table: "chatsession",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_degree_facultyid",
                table: "degree",
                column: "facultyid");

            migrationBuilder.CreateIndex(
                name: "IX_degreemodule_moduleid",
                table: "degreemodule",
                column: "moduleid");

            migrationBuilder.CreateIndex(
                name: "IX_enrollment_studentid",
                table: "enrollment",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_enrollmentdegree_degreeid",
                table: "enrollmentdegree",
                column: "degreeid");

            migrationBuilder.CreateIndex(
                name: "IX_moduleresource_moduleid",
                table: "moduleresource",
                column: "moduleid");

            migrationBuilder.CreateIndex(
                name: "IX_reply_forumtopicid",
                table: "reply",
                column: "forumtopicid");

            migrationBuilder.CreateIndex(
                name: "IX_sessionrating_ratingid",
                table: "sessionrating",
                column: "ratingid");

            migrationBuilder.CreateIndex(
                name: "IX_sessionstudent_studentid",
                table: "sessionstudent",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "IX_sessiontutor_tutorid",
                table: "sessiontutor",
                column: "tutorid");

            migrationBuilder.CreateIndex(
                name: "IX_studenttutor_tutorid",
                table: "studenttutor",
                column: "tutorid");

            migrationBuilder.CreateIndex(
                name: "IX_topicmodule_clusterid",
                table: "topicmodule",
                column: "clusterid");

            migrationBuilder.CreateIndex(
                name: "IX_topicmodule_moduleheadid",
                table: "topicmodule",
                column: "moduleheadid");

            migrationBuilder.CreateIndex(
                name: "IX_tutors_specialityid",
                table: "tutors",
                column: "specialityid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chatmessages");

            migrationBuilder.DropTable(
                name: "degreemodule");

            migrationBuilder.DropTable(
                name: "enrollmentdegree");

            migrationBuilder.DropTable(
                name: "MessageUsers");

            migrationBuilder.DropTable(
                name: "moduleresource");

            migrationBuilder.DropTable(
                name: "reply");

            migrationBuilder.DropTable(
                name: "sessionrating");

            migrationBuilder.DropTable(
                name: "sessionstudent");

            migrationBuilder.DropTable(
                name: "sessiontutor");

            migrationBuilder.DropTable(
                name: "studenttutor");

            migrationBuilder.DropTable(
                name: "chatsession");

            migrationBuilder.DropTable(
                name: "degree");

            migrationBuilder.DropTable(
                name: "enrollment");

            migrationBuilder.DropTable(
                name: "topicmodule");

            migrationBuilder.DropTable(
                name: "forumtopic");

            migrationBuilder.DropTable(
                name: "rating");

            migrationBuilder.DropTable(
                name: "session");

            migrationBuilder.DropTable(
                name: "faculty");

            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "modulecluster");

            migrationBuilder.DropTable(
                name: "tutors");

            migrationBuilder.DropTable(
                name: "speciality");
        }
    }
}
