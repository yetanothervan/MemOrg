using System.Collections.Generic;
using DAL.Entity;

namespace EF
{
    static internal class SeedSample
    {
//        public static void Seed(MemOrgContext context)
//        {
//            var p1 = new SourceTextParticle
//            {
//                Content = @"%����� - �� �����. �.�. ��� ���� �� ������ ���� �� ������ ������������. ��� ���� %�������.",
//                Order = 0
//            };
//            var p2 = new SourceTextParticle
//            {
//                Content = @"������� - ��� ������ ��������� �������� �����������, � �������� ��������� � %����� ����.",
//                Order = 1
//            };
//            var p3 = new SourceTextParticle
//            {
//                Content =
//                    @"������� ���� �������� ������ � ����� - �������� �����. �������� �����: ������� ��������� 10-15%, ���������, ������� ���������, �������. � ����� �� ������� ����� 30-40%. 
//������� ����� ������, ������������, �� �����, ������� ����� �������� �� ����������. ���������� �� ����� ����� ������ ��������� ���������������. 
//���� ������� �� ������� ����� ������ ������ ���, ����� ��������� ������, � ��� ���������� - �����. ��, ��� ���� ����� ����� - �������� ��������� �����, ��� � ����������.
// � ������ ����� ������� �� �����. � �������������� � ������ - ������ ��������� �� ����������� � �� �������� ������.
// � ������ ������� ����� ����. 
//
//��� ������ �� ������ ��� �� �� ���� ����� � ������ ������������ ��������, ��������� � %��� ����� %���������. 
//���� ���������� (� ��� - %���������� ����� ��� �� %��������), ���� ���������� �����������, ��������� � �����, � ���������� (� ��� - ����������� ������, � ������������ ��������� - �������). � ���� ������ ���� - ""�� ����"" ��� - ��������� ������ ���. ������ � ������ - ����� ������, � ������ ����� ���.
// � ���������� ����� ��������� � ���, � �������� �������� ���������� �����������. ",
//                Order = 2
//            };
//            var p8 = new SourceTextParticle
//            {
//                Content =
//                    @"��� %��������������� �������� ���������� �� ����������? ���, ��� ��������� - ��� ������������, ����������, � ��������������� - ��� ����� ��������, ���������� ������� �����������, �������� � �.�. ������ ���� ����.",
//                Order = 3
//            };
//            var p9 = new SourceTextParticle
//            {
//                Content =
//                    @"���������� ���������� (""������"" � ""��������"") ����� ����������� ������ � �������� ����������. ���� ����������� ����� � �������� %�����, �� ���� ����� 3-4 ����� ������ ������� � ����� ��������. ����� �� ������ ������� �� ��������, � �� ������ �������� � ������.",
//                Order = 4
//            };
//            var p10 = new SourceTextParticle
//            {
//                Content =
//                    @"%����� ����� ���� ������ - ��������, �������������������, ������������ � �����. ���������� ���� ������� - �������������. ��� ����� ������������ ����������� - � ����� ������ �������� ������� � ������ ������������ �������. ������ ���� ��������, ���������. ��������� ��������� ������������� ��������� (����������� �������� � ��.) ����������� �������� � ����������� ���������� ����������, ��������� ������� ��������������� ���������. ������������� ��������� ����� %""������� ����������� � �����"".",
//                Order = 5
//            };
//            var p4 = new SourceTextParticle
//            {
//                Content = @"������� ������ ����� ��������. 
//����� ���� - ������� �� ��������.
//������ ����. ������� - ������.
//�� �� �������� ���������. 
//	/�������� �����/
//
// %������� - ����� ������ ����. ������� �� �������� �����. �� ����� ������������� � �����, � �������� �� ����� ������ ������������� ������.",
//                Order = 6
//            };
//            var p5 = new SourceTextParticle
//            {
//                Content =
//                    @"�������� �� ������ ����� � �����, ��� ����� ��������. � ������� �� ��������� ���������, ��������� �� �������� ������� ����� ��������, ���������� ���������, �� ����� ������� ������� ������������� � �.�.
//
//������ ���� (""�� ����"" � ��� ��� ����� � ������) ��������������, ����� ���������� ������� � ��� �� ���� ������� ���������. 
//��������� � ���, ��� � �������. � ���� ������ ""�� ����"" ����������, ��� ����� ���������, ��������� ����� ������, � ���������� - �����������. ""�� ����"" �������� ������ � ������� ���������� �������� - ��������� ��������.
//�������� ������ � ��� ����� �� ��������: ""� �� ��� �������� ���������� ���������, ������� �������� �� ����� ������������� ����������� ����, � �������� ��� ����������� ��������� ��������� �����? ��� �� �������� ������ ������������� �����������! ��� ������������ - ���������� ��������� ����� ���������� � �� �������������� ""�� ����"", ���� ���������� ��������������� ����� ���������� � ���������, ��� � ����. � ��, ��� ���� ���������� ��������� ������ ��� ����� �������� �������������, ��������� ������� �� ��������� � ��������, ������� ���������, ������� ����������� ���������� � ���������� ��������? ��� ���� ������� ������ �� ������ � ������������, �� � � �����������������, ������������� �����.""
//
//������ �������� - ������ ������������� ��� ����� � ��� ������������ � ���� �����. ��� ����������� ��� ����. ��� �����������. ��� ""�� ����"" - ������ ���������. �������� ���������� ���������������� �����������, � ������������ ���������������� ��������� ���������� ������� �������� ��� �� ������������� �� ���� �������.
//
//����� ���� ���������� ����� �������������� �� ������ ������. �� ����, �� �����, �����, ������ � �.�. ����� ����� ������ - ������. ���",
//                Order = 7
//            };
//            var p12 = new SourceTextParticle
//            {
//                Content = @"%������� ������� ������ ""�� ������� ��� � �������������"".",
//                Order = 8
//            };
//            var p6 = new SourceTextParticle
//            {
//                Content =
//                    @"������� ��� ���������� �� �������� � ������������� ������ �����, �� ��������� ��������� �� ������� ��� ������, ��� ������ ���������� �������.
//��� ���������� �� ������ ���������, ���������� � ������ ��������� ��� ��������������� � ��������. �.�. �������������, ������� �� ���� �����, ������� � ��� �������� ����� � ��������� ������.
// ���������� �� ����������, ��� �� ���������� �������������, ����� ������ �������� � �������. ��� ���������-����������� ����� ����� ��������� ��� ���������� � ���-�� ���������� ����������������, ����������� ����������������� ����������� ������������.. �� ������� �� ��������� ��������� � ����������, ��� ����� ����������� � ������������� ����������� ������������... ��� � ���� %�������. � %����.",
//                Order = 9
//            };
//            var p7 = new SourceTextParticle
//            {
//                Content =
//                    @"[��� ���������] ���� ������� � ����� ����� ��������� ��� ������� ���� - ��� ������ ���� ������ ����, � �� %���������. 
// ��������� - ��� ����, ���������� ����� �������. ������ ��������, ����������� ����� �����������, ��� ����������. ���� - ��� ���������� ����.
//��������� ������������ ������� ���������, ��������� �� ����������� �����, �� ����������� ���. � �������� �������� ������. ",
//                Order = 10
//            };
//            var p11 = new SourceTextParticle
//            {
//                Content =
//                    @"""�� ���� �� ��������� ��������������, ��� ��������� ������ �����������, ����� ������ � ��, � ���������, � ��������� ��� ������� ����?... ����� �������������� ������ ���� ���������� ��� ����. ��� ������ ������ �� �����������...
//�������� � ���� ������ ""��������� ������"" � ����������� � ����������. � �� �������, ��� ��� ������ ������ ����� ������� ""��������� �����""... 
//������ ��� ��� ������� ����������� ���������, � �������� �������""",
//                Order = 11
//            };

//            var b1 = new Block { Caption = "���������� �����, �������� � �������" };
//            var b2 = new Block { Caption = "���" };
//            ;
//            var b3 = new Block { Caption = "������� ����������� � �����" };

//            b1 = context.Blocks.Add(b1);
//            b2 = context.Blocks.Add(b2);
//            b3 = context.Blocks.Add(b3);

//            var r1 = context.References.Add(new Reference { ReferencedBlock = b1 });
//            var r2 = context.References.Add(new Reference { ReferencedBlock = b1 });
//            var r3 = context.References.Add(new Reference { ReferencedBlock = b2 });
//            var r4 = context.References.Add(new Reference { ReferencedBlock = b3 });

//            context.Blocks.Add(
//                new Block
//                {
//                    Caption = "�� ��������� �� ��������� - 4",
//                    Particles = new List<Particle> { p1, p2, p3, p8, p9, p10, p4, p5, p12, p6, p7, p11 },
//                    References = new List<Reference> { r1, r2, r3, r4 }
//                });
//            context.Blocks.Add(new Block
//            {
//                Caption = "�����",
//                Particles = new List<Particle> { new QuoteSourceParticle { SourceTextParticle = p1, Order = 0 }, new QuoteSourceParticle { SourceTextParticle = p2, Order = 1 } }
//            });
//            context.Blocks.Add(new Block
//            {
//                Caption = "����� ����",
//                Particles =
//                    new List<Particle>
//                    {
//                        new SourceTextParticle {Content = "��������� �����", Order = 0},
//                        new QuoteSourceParticle {SourceTextParticle = p2, Order = 1}
//                    }
//            });
//            context.Blocks.Add(new Block
//            {
//                Caption = "�������",
//                Particles = new List<Particle> { new QuoteSourceParticle { SourceTextParticle = p2, Order = 0 } }
//            });
//            context.Blocks.Add(new Block
//            {
//                Caption = "�������, �������� ���������",
//                Particles = new List<Particle>
//                {
//                    new SourceTextParticle
//                    {
//                        Content = "������������ �� ����",
//                        Order = 0
//                    },
//                    new QuoteSourceParticle {SourceTextParticle = p4, Order = 1},
//                    new QuoteSourceParticle {SourceTextParticle = p6, Order = 2}
//                }
//            });
//            context.Blocks.Add(new Block
//            {
//                Caption = "��������������� ��������",
//                Particles = new List<Particle> { new QuoteSourceParticle { SourceTextParticle = p8, Order = 0 } }
//            });
//            context.Blocks.Add(new Block
//            {
//                Caption = "�����",
//                Particles = new List<Particle> { new QuoteSourceParticle { SourceTextParticle = p10, Order = 0 } }
//            });
//            context.Blocks.Add(new Block
//            {
//                Caption = "�������, ����",
//                Particles = new List<Particle>
//                {
//                    new SourceTextParticle
//                    {
//                        Content = "42-� ��������� ��� (1993 - 2001)",
//                        Order = 0
//                    },
//                    new QuoteSourceParticle {SourceTextParticle = p12, Order = 1}
//                }
//            });
//            context.Blocks.Add(new Block
//            {
//                Caption = "��������� ������",
//                Particles = new List<Particle>
//                {
//                    new SourceTextParticle
//                    {
//                        Content = "���������� (����� - ���������) ������",
//                        Order = 0
//                    },
//                    new QuoteSourceParticle {SourceTextParticle = p11, Order = 1}
//                }
//            });
//            context.SaveChanges();
//        }
    }
}