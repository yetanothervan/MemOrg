using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace EF
{
    public class SeedConceptualWar
    {
        public static void Seed(MemOrgContext context)
        {
            SeedChapter1(context);
        }

        private static void SeedChapter1(MemOrgContext context)
        {
            #region Define particles

            var abelarParticle = new SourceTextParticle
            {
                Content =
                    @"французский философ, теолог, поэт. Развил учение, названное концептуализмом. Разрабатывал схоластическую диалектику (сочинение «Да и нет»).  Рационалистическая направленность Абеляра вызвала протест ортодоксальных церковных кругов. Религиозное учение Абеляра состояло в том, что Бог дал человеку все силы для достижения благих целей, следовательно и ум, чтобы удержать в пределах игру воображения и направлять религиозное верование. Вера, говорил он, зиждется непоколебимо только на убеждении, достигнутом путем свободного мышления; а потому вера, приобретенная без содействия умственной силы и принятая без самодеятельной проверки, недостойна свободной личности. Абеляр утверждал, что единственными источниками истины являются диалектика и Священное писание, и что даже апостолы и отцы Церкви могли заблуждаться.",
                Order = 0
            };

            var conceptWarParticle = new SourceTextParticle
            {
                Content = @"Понятие «Концептуальная война» состоит из 2-х слагаемых: концепт и война.. ",
                Order = 1
            };

            var conceptParticle = new SourceTextParticle
            {
                Content =
                    @"Слово «концепт» (от латинского conceptus – «зародыш» ) вошло в научный оборот в европейском средневековье в религиозно-научных дискуссиях на тему о возможности и содержании так называемых универсалий – общих понятий о вещах, событиях и т.д. А также о соотношении единичного и всеобщего и их отражение в человеческом сознании и языке.
Наиболее полно понятие «концепт» разработано Пьером Абеляром, который пытался разрешить спор между сторонниками реализма и номинализма.",
                Order = 2
            };

            var abelarAboutConceptParticle = new SourceTextParticle
            {
                Content =
                    @"Абеляр, не принимая крайностей реализма и номинализма, утверждал, что универсалии создаются человеком на основе чувственного опыта в результате объединения в его уме тех свойств вещей и событий, которые действительно являются для них общими. Этот процесс назван концептуализацией, а созданная универсалия – концептом.",
                Order = 3
            };

            var conceptParticleAdd1 = new SourceTextParticle
            {
                Content =
                    @"Концептуализация – это объединение комплекса идей и представлений, относящихся к какой-либо вещи, проблеме, процессу, в целостную единую систему – концепт. Методы такого объединения могут включать в себя разные формы осмысления: логический анализ общего в комплексе идей и представлений и выстраивание между ними логических связей, интуитивное прозрение («схватывание») этого общего, образное сопряжение/объединение идей и представлений в умопостигаемую целостность (различными, в том числе интеллектуальными и художественными, средствами) и т.д. ",
                Order = 4
            };

            var philosophyOfHistoryParticle = new SourceTextParticle
            {
                Content =
                    @"Центральным вопросом спора между реалистами, номиналистами и концептуалистами был вопрос о человеке в истории. В том числе, о его развитии как воплощении замысла Творца, о его способностях к познанию богоданной реальности и её общих свойств. И роли человека в божественном промысле – предопределенной божественной волей (но неясно, в какой мере предопределенной!) исторической драме движения Творения от акта созидания к историческому финалу.

Соответственно, концепты и концептуализация в их высшем и самом важном регистре, с религиозно-философской точки зрения, относилось к Истории Творения и роли Человека (человечества) в этой Истории. То есть к той философской проблематике, которую назвали позже философией истории. ",
                Order = 5
            };

            var conceptWarParticleAdd1 = new SourceTextParticle
            {
                Content =
                    @"Главная роль концептов – организация нашего мышления. Именно концепты, обеспечивают ту «смысловую оптику», через которую мы осмысливаем окружающую нас материальную, социальную, идеальную реальность. И далее, восприняв и осмыслив эту реальность через оптику воспринятых концептов, организуем свою деятельность в реальности в соответствии и этими концептами. 

Приоритетом нашего внимания в части рассмотрения концептов и концептуализации – будут представления об Истории (её наличии и смысле), о развитии в истории, о человеке, развивающемся в истории, действующем в истории и сопричастном (или вовсе не сопричастном) творению этой истории. Т.е. первоочередной предмет нашего исследования, его главная территория – это история и роль человека в истории.

Концептуальная война – это война за организацию «мыслительной оптики» человека. Соответственно, «военные» концепты – это механизмы разрушительной (смещенной, искаженной, ущербной) организации мышления адресатов таких концептов. А заодно – инструменты оправдания действий тех, кто эти военные концепты создал и внедрил. Цель создания военных концептов – навязывание противнику такой разрушительной «мыслительной оптики», которая нужна (выгодна) создателю концепта. ",
                Order = 6
            };

            var userTextParticle = new UserTextParticle
            {
                Content =
                    @"Концептуальная война – это война за организацию «мыслительной оптики» человека. Соответственно, «военные» концепты – это механизмы разрушительной (смещенной, искаженной, ущербной) организации мышления адресатов таких концептов. А заодно – инструменты оправдания действий тех, кто эти военные концепты создал и внедрил. Цель создания военных концептов – навязывание противнику такой разрушительной «мыслительной оптики», которая нужна (выгодна) создателю концепта.",
                Order = 0
            };

            #endregion

            #region Define tags

            var humanTag = CreateTag("Человек", null);
            var philosopherTag = CreateTag("Философ", humanTag);
            var sciensistTag = CreateTag("Ученый", humanTag);
            var teologTag = CreateTag("Теолог", sciensistTag);
            var nationalityTag = CreateTag("Национальность", humanTag);
            var frenchTag = CreateTag("Француз", nationalityTag);
            var literatorTag = CreateTag("Литератор", humanTag);
            var poetmanTag = CreateTag("Поэт", literatorTag);
            var etcTag = CreateTag("ЭТЦ", humanTag);

            var kafedraTag = CreateTag("Кафедра", null);
            var kafedraConceptualWarTag = CreateTag("Концептуальная война", kafedraTag);
            kafedraConceptualWarTag.TagBlock = new Block();
            var bookConceptualWarTag = CreateTag("«Концептуальная война»", kafedraConceptualWarTag);
            bookConceptualWarTag.TagBlock = new Block();

            var theWordTag = CreateTag("Понятие", null);
            var eotTag = CreateTag("СВ", theWordTag);

            #endregion

            //на параметры пока забьем

            #region Define blocks

            var abelarBlock = new Block
            {
                Caption = "Абеляр, Пьер Пале",
                Particles =
                    new List<Particle> {new QuoteSourceParticle {SourceTextParticle = abelarParticle, Order = 0}},
                Tags = new List<Tag> {teologTag, frenchTag, poetmanTag, philosopherTag}
            };

            var conceptWarBlock = new Block
            {
                Caption = "Концептуальная война",
                Particles =
                    new List<Particle>
                    {
                        new QuoteSourceParticle {SourceTextParticle = conceptWarParticle, Order = 0},
                        new QuoteSourceParticle {SourceTextParticle = conceptWarParticleAdd1, Order = 1}
                    },
                Tags = new List<Tag> {eotTag}
            };

            var conceptBlock = new Block
            {
                Caption = "Концепт",
                Particles =
                    new List<Particle>
                    {
                        new QuoteSourceParticle {SourceTextParticle = conceptParticle, Order = 0},
                        new QuoteSourceParticle {SourceTextParticle = conceptParticleAdd1, Order = 1}
                    },
                Tags = new List<Tag> {eotTag}
            };

            var abelarAboutConceptBlock = new Block
            {
                Caption = "Абеляр про Концепт",
                Particles =
                    new List<Particle>
                    {
                        new QuoteSourceParticle {SourceTextParticle = abelarAboutConceptParticle, Order = 0}
                    }
            };

            var philosofyOfHistiry = new Block
            {
                Caption = "Философия истории",
                Particles =
                    new List<Particle>
                    {
                        new QuoteSourceParticle {SourceTextParticle = philosophyOfHistoryParticle, Order = 0}
                    },
                Tags = new List<Tag> {eotTag}
            };

            var utConceptWarBlock = new Block
            {
              Caption   = "Размышления о концептуальной войне",
              Particles = new List<Particle>{userTextParticle}
            };

            var byalyBlock = new Block
            {
                Caption = "Бялый, Юрий",
                Tags = new List<Tag> {etcTag}
            };

            var cwChapter1 = new Block
            {
                Caption = "Введение",
                Particles =
                    new List<Particle>
                    {
                        abelarParticle,
                        conceptWarParticle,
                        conceptParticle,
                        abelarAboutConceptParticle,
                        conceptParticleAdd1,
                        philosophyOfHistoryParticle,
                        conceptWarParticleAdd1
                    },
                Tags = new List<Tag> {bookConceptualWarTag}
            };

            #endregion

            #region Define relations

            var aboutRelationType = new RelationType {Caption = "о"};
            var abelarAboutConceptRelation = new Relation
            {
                FirstBlock = abelarBlock,
                SecondBlock = conceptBlock,
                RelationBlock = abelarAboutConceptBlock,
                RelationType = aboutRelationType
            };

            var authorRelationType = new RelationType {Caption = "Автор"};
            var byalyAuthorConceptWarBookRelation = new Relation
            {
                FirstBlock = byalyBlock,
                SecondBlock = bookConceptualWarTag.TagBlock,
                RelationType = authorRelationType
            };

            #endregion

            #region Define references

            philosofyOfHistiry.References = new List<Reference> {new Reference {ReferencedBlock = conceptBlock}};
            utConceptWarBlock.References = new List<Reference> {new Reference {ReferencedBlock = conceptWarBlock}};
            CreateDoubleEndedReference(conceptWarBlock, kafedraConceptualWarTag.TagBlock);
            CreateDoubleEndedReference(conceptWarBlock, conceptBlock);

            #endregion

            context.Blocks.Add(abelarBlock);
            context.Blocks.Add(conceptWarBlock);
            context.Blocks.Add(conceptBlock);
            context.Blocks.Add(abelarAboutConceptBlock);
            context.Blocks.Add(philosofyOfHistiry);
            context.Blocks.Add(utConceptWarBlock);
            context.Blocks.Add(byalyBlock);
            context.Blocks.Add(cwChapter1);

            context.Relations.Add(abelarAboutConceptRelation);
            context.Relations.Add(byalyAuthorConceptWarBookRelation);
        }

        private static Tag CreateTag(string caption, Tag parent)
        {
            return new Tag {Parent = parent, Caption = caption};
        }

        private static void CreateDoubleEndedReference(Block first, Block second)
        {
            if (first.References == null)
                first.References = new List<Reference>();
            if (second.References == null)
                second.References = new List<Reference>();
            first.References.Add(new Reference {ReferencedBlock = second});
            second.References.Add(new Reference {ReferencedBlock = first});
        }
    }
}