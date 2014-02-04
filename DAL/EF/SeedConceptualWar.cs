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

            int pOrder = -1;
            var conceptWarParticle = new SourceTextParticle
            {
                Content = @"Понятие «Концептуальная война» состоит из 2-х слагаемых: концепт и война. ",
                Order = ++pOrder
            };

            var conceptParticle = new SourceTextParticle
            {
                Content =
                    @"Слово «концепт» (от латинского conceptus – «зародыш» ) вошло в научный оборот в европейском средневековье в религиозно-научных дискуссиях на тему о возможности и содержании так называемых универсалий – общих понятий о вещах, событиях и т.д. А также о соотношении единичного и всеобщего и их отражение в человеческом сознании и языке.
Наиболее полно понятие «концепт» разработано Пьером Абеляром, который пытался разрешить спор между сторонниками реализма и номинализма.",
                Order = ++pOrder
            };

            var abelarAboutConceptParticle = new SourceTextParticle
            {
                Content =
                    @"Абеляр, не принимая крайностей реализма и номинализма, утверждал, что универсалии создаются человеком на основе чувственного опыта в результате объединения в его уме тех свойств вещей и событий, которые действительно являются для них общими. Этот процесс назван концептуализацией, а созданная универсалия – концептом.",
                Order = ++pOrder
            };

            var conceptParticleAdd1 = new SourceTextParticle
            {
                Content =
                    @"Концептуализация – это объединение комплекса идей и представлений, относящихся к какой-либо вещи, проблеме, процессу, в целостную единую систему – концепт. Методы такого объединения могут включать в себя разные формы осмысления: логический анализ общего в комплексе идей и представлений и выстраивание между ними логических связей, интуитивное прозрение («схватывание») этого общего, образное сопряжение/объединение идей и представлений в умопостигаемую целостность (различными, в том числе интеллектуальными и художественными, средствами) и т.д. ",
                Order = ++pOrder
            };

            var philosophyOfHistoryParticle = new SourceTextParticle
            {
                Content =
                    @"Центральным вопросом спора между реалистами, номиналистами и концептуалистами был вопрос о человеке в истории. В том числе, о его развитии как воплощении замысла Творца, о его способностях к познанию богоданной реальности и её общих свойств. И роли человека в божественном промысле – предопределенной божественной волей (но неясно, в какой мере предопределенной!) исторической драме движения Творения от акта созидания к историческому финалу.

Соответственно, концепты и концептуализация в их высшем и самом важном регистре, с религиозно-философской точки зрения, относилось к Истории Творения и роли Человека (человечества) в этой Истории. То есть к той философской проблематике, которую назвали позже философией истории. ",
                Order = ++pOrder
            };

            var conceptWarParticleAdd1 = new SourceTextParticle
            {
                Content =
                    @"Главная роль концептов – организация нашего мышления. Именно концепты, обеспечивают ту «смысловую оптику», через которую мы осмысливаем окружающую нас материальную, социальную, идеальную реальность. И далее, восприняв и осмыслив эту реальность через оптику воспринятых концептов, организуем свою деятельность в реальности в соответствии и этими концептами. 

Приоритетом нашего внимания в части рассмотрения концептов и концептуализации – будут представления об Истории (её наличии и смысле), о развитии в истории, о человеке, развивающемся в истории, действующем в истории и сопричастном (или вовсе не сопричастном) творению этой истории. Т.е. первоочередной предмет нашего исследования, его главная территория – это история и роль человека в истории.

Концептуальная война – это война за организацию «мыслительной оптики» человека. Соответственно, «военные» концепты – это механизмы разрушительной (смещенной, искаженной, ущербной) организации мышления адресатов таких концептов. А заодно – инструменты оправдания действий тех, кто эти военные концепты создал и внедрил. Цель создания военных концептов – навязывание противнику такой разрушительной «мыслительной оптики», которая нужна (выгодна) создателю концепта. ",
                Order = ++pOrder
            };

            var abelarNameParticle = new SourceTextParticle
            {
                Content =
                    @"Абеляр, Пьер Пале",
                Order = ++pOrder
            };

            var abelarParticle = new SourceTextParticle
            {
                Content =
                    @"французский философ, теолог, поэт. Развил учение, названное концептуализмом. Разрабатывал схоластическую диалектику (сочинение «Да и нет»).  Рационалистическая направленность Абеляра вызвала протест ортодоксальных церковных кругов. Религиозное учение Абеляра состояло в том, что Бог дал человеку все силы для достижения благих целей, следовательно и ум, чтобы удержать в пределах игру воображения и направлять религиозное верование. Вера, говорил он, зиждется непоколебимо только на убеждении, достигнутом путем свободного мышления; а потому вера, приобретенная без содействия умственной силы и принятая без самодеятельной проверки, недостойна свободной личности. Абеляр утверждал, что единственными источниками истины являются диалектика и Священное писание, и что даже апостолы и отцы Церкви могли заблуждаться.",
                Order = ++pOrder
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
                        conceptWarParticle,
                        conceptParticle,
                        abelarAboutConceptParticle,
                        conceptParticleAdd1,
                        philosophyOfHistoryParticle,
                        conceptWarParticleAdd1,
                        abelarNameParticle,
                        abelarParticle
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

            philosofyOfHistiry.References = new List<Reference>
            {
                new Reference {ReferencedBlock = conceptBlock}
            };
            conceptWarBlock.References = new List<Reference>
            {
                new Reference {ReferencedBlock = utConceptWarBlock}
            };
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

            context.SaveChanges();
        }

        private static void SeedChapter2(MemOrgContext context)
        {
            #region Define particles
            int pOrder = -1;
            
            var articleEndOfHistoryParticle = new SourceTextParticle
            {
                Content = @"Френсис Фукуяма в 1989 г. опубликовал в американском журнале National Interest статью «Конец истории?». А затем и книгу «Конец истории и последний человек». И я знаю, что содержание статьи Фукуямы, а также ее беспрецедентная «раскрутка» во всем мире – это полноценная военная операция. Причем не идеологическая, а именно концептуальная. ",
                Order = ++pOrder
            };
            
            var relDifferentConceptWarIdeologicalWarParticle = new SourceTextParticle
            {
                Content = @"Идеологическая война оперирует такими понятиями, как коммунизм, либерализм, консерватизм, фашизм и так далее. Концептуальная война ведется на следующем этаже. Каком же именно?
И коммунизм, и либерализм, и фашизм, и прочие «измы», борьба между которыми именуется идеологической войной, не посягают на историю. Они стремятся направить исторический поток в то или иное русло.
Концептуальная же война – это не война за русло, в котором будет бурлить историческая энергия. Это война против истории как таковой.",
                Order = ++pOrder
            };

            var conceptEndOfHistoryParticle = new SourceTextParticle
            {
                Content = @"Фукуяма как бы заявляет борющимся идеологам: «Вы все умерли. Не только фашисты и коммунисты, но и победившие в этой войне либералы. Ибо с победой либералов история кончилась». Мол, если СССР проиграл, и нет коммунизма, если перед этим проиграл фашизм – то в мире стран победившего либерализма, которые способны влиять на историческое движение, история кончилась. Они – в постистории. Конечно, есть еще периферия мира, где пока что конфликтуют и воюют реликты «исторического». Но постисторический мир в конечном итоге и их сведет к единому «постисторическому знаменателю».
Скромный труд Фукуямы порожден концептуальной войной, которую, по определению, могут вести даже не просто сильные мира сего, а наисильнейшие этого самого мира.",
                Order = ++pOrder
            };

            var sourceParticle1Particle = new SourceTextParticle
            {
                Content = @"Фукуяма никогда не скрывал, что является учеником Александра Кожева – эмигранта из России, ставшего одним из крупных французских философов. Сам Кожев всегда подчеркивал, что он развивает определенные стороны учения великого немецкого философа Гегеля. А поскольку гегельянцев и неогегельянцев в мире пруд пруди, то важно понять, какие именно аспекты гегелевского учения развивал Кожев.",
                Order = ++pOrder
            };

            var relKojevAboutGegelParticle = new SourceTextParticle
            {
                Content = @"Кожев размышлял над очень пунктирно высказанной Гегелем идеей перехода исторического духа к постисторической фазе, на которой этот самый исторический дух сменяется «Новым духом». Почему же исчезает исторический дух? Потому что он реализовал весь потенциал исторической новизны. Новизна становится в мире невозможной. Все новое высказано. А раз так, то пора приступать к его каталогизации.
Все идеи, мысли и представления становятся клеточками одного огромного каталога. Подчеркну еще раз, что до момента, пока новизна возможна, заниматься ее каталогизацией бессмысленно. Только ты разложил все по полочкам – бац, появилось что-то новое. И начинай строгать новую полочку, приколачивать ее к огромному стеллажу. Только завершил этот этап, перевел дух и обозрел содеянное, – на тебе, новая новизна. Нет уж, давайте сначала исчерпаем потенциал новизны и лишь потом начнем каталогизировать.
Исчерпали новизну, начали каталогизировать – что дальше? Гегель нигде не говорит об этом прямо, но по некоторым более ими менее смутным фрагментам можно реконструировать его мысль следующим образом. Исторический дух умер, каталогизация проведена, и Новый дух начинает играть элементами этой каталогизации сообразно некоторым выявленным игровым правилам. История кончилась. И началась Игра. ",
                Order = ++pOrder
            };

            var relBookIgraVBiserAboutGameParticle = new SourceTextParticle
            {
                Content = @"Он не просто играет – он в этой игре изучает предельные тайны композиции между существующими элементами. Именно об этом писал Герман Гессе в романе «Игра в бисер», описывая гроссмейстеров подобной игры. ",
                Order = ++pOrder
            };

            var relFukuyamaAboutElitsParticle = new SourceTextParticle
            {
                Content = @"Так что же именно навязывали Фукуяма и стоящая за ним интеллектуальная система, в которой Кожев – лишь одно из важных колесиков? Они, по большому счету, навязывали мысль о всевластности элиты в постисторическом мире. ",
                Order = ++pOrder
            };

            var utAboutEndOfHistoryParticle = new UserTextParticle
            {
                Content = @"Так что же именно навязывали Фукуяма и стоящая за ним интеллектуальная система, в которой Кожев – лишь одно из важных колесиков? Они, по большому счету, навязывали мысль о всевластности элиты в постисторическом мире. ",
                Order = 0
            };

            var relElitsAndHistoricalFireParticle = new SourceTextParticle
            {
                Content = @"Элита есть всегда. И всегда играет в разного рода тонкие игры. Особенно в этом всегда преуспевали закрытые элитные группы, высшие спецслужбы, представители аристократических сообществ. Беспокоило эти сообщества только одно – играешь, играешь, и вдруг слышишь рев под окнами. Колоссальная масса «плебса» – народа, возбужденного каким-то новым идеалом, воспламененного огнем новой идеи, накатывается на тебя, и тебе приходится жертвовать частью собственного драгоценного класса.
Другая часть этого класса имитирует подчинение воле плебса и его вожаков – и ждет, пока огонь погаснет. После этого можно снова переходить к Игре, надеясь, что теперь-то она всевластна. Так ведь нет, возгорается новый огонь, и новый плебс бушует под окнами.
Возникает аристократическая мечта о том, чтобы погасить огонь однажды и навсегда. Она же – мечта о конце истории. В этом смысле конец истории равносилен невозможности возникновения новых исторических проектов, равносилен неспособности каких-либо крупных человеческих масс воспламеняться огнем любви к новому историческому идеалу. То есть идеалу, который, будучи новым, одновременно является и сокровенно преемственным по отношению к идеалам предшествующим",
                Order = ++pOrder
            };

            var utAboutHistoricalFireAndAboutElitsParticle = new UserTextParticle
            {
                Content = @"Возникает аристократическая мечта о том, чтобы погасить огонь однажды и навсегда. Она же – мечта о конце истории. В этом смысле конец истории равносилен невозможности возникновения новых исторических проектов, равносилен неспособности каких-либо крупных человеческих масс воспламеняться огнем любви к новому историческому идеалу. То есть идеалу, который, будучи новым, одновременно является и сокровенно преемственным по отношению к идеалам предшествующим.",
                Order = 0
            };

            var sourceParticle2Particle = new SourceTextParticle
            {
                Content = @"Так ли уж неоснователен тезис Фукуямы о конце истории и начале постистории? Какие крупные человеческие массы способны возгореться огнем любви к новому историческому идеалу? И каков он, этот новый исторический идеал?",
                Order = ++pOrder
            };

            var relHistoricalFireAndEvropaParticle = new SourceTextParticle
            {
                Content = @"Может ли возгореться реальная Европа? Все возможно, но на сегодняшний день Европа совсем лишена исторического огня. Существенная часть европейского населения способна возмутиться, если капиталисты начнут отбирать социальные завоевания у трудящихся. Но подобное возмущение далеко от исторического огня. ",
                Order = ++pOrder
            };

            var relHistoricalFireAndUsaParticle = new SourceTextParticle
            {
                Content = @"В США ситуация ненамного лучше. Там есть остатки идеологической страсти по «Граду на холме». Но это те самые реликты, о которых писал Фукуяма. Они могут носить консервативный или религиозно-фундаменталистский характер, но не имеют ничего общего с новой «перегретой» исторической страстью, необходимой для продолжения истории. ",
                Order = ++pOrder
            };

            var relHistoricalFireAndChinaParticle = new SourceTextParticle
            {
                Content = @"Китай – достаточно холоден и прагматичен, как и вся Юго-Восточная Азия. Существенная часть индийского населения религиозно накалена, но эта страсть неисторична в том смысле, который мы сейчас обсуждаем. ",
                Order = ++pOrder
            };

            var relHistoricalFireAndMuslimParticle = new SourceTextParticle
            {
                Content = @"Перечисляя все это, естественно, натыкаешься на ислам. Тут есть и огненная страсть, и масштаб. Но тут опять-таки нет главного – исторической новизны. Нет настоящей воли к продолжению истории.
Тем не менее, именно наличие огненного ислама (а его огонь отрицать невозможно) привело к тому, что в начале XXI века Фукуяма отказался от концепции конца истории.
Однако, фундаменталистский ислам не обеспечивает нового исторического огня. ",
                Order = ++pOrder
            };

            var relHistoricalFireAndLatinAmericaParticle = new SourceTextParticle
            {
                Content = @"Но где же тогда может этот огонь возгореться? В Латинской Америке? Да, этот во многом загадочный континент наполнен страстью и разнообразными идеологическими исканиями. Но пока совершенно не ясно, способен ли он к полноценному идеологическому воспламенению. ",
                Order = ++pOrder
            };

            var relHistoricalFireAndRussiaParticle = new SourceTextParticle
            {
                Content = @"А значит, остается Россия. Не потому, что нам так хочется, а исходя из объективной «карты исторических температур». Да, красный огонь, которым Россия воспламенилась в 1917 году и который спас историю, удалось погасить в ходе так называемой перестройки. Но до конца ли удалось погасить?
Мир без истории не просто мрачен. Он тосклив и уродлив одновременно.",
                Order = ++pOrder
            };

            var nameFukuyamaParticle = new SourceTextParticle
            {
                Content = @"Фукуяма, Френсис",
                Order = ++pOrder
            };

            var fukuyamaParticle = new SourceTextParticle
            {
                Content = @"(р.1952) американский философ, политолог и социолог ",
                Order = ++pOrder
            };

            var nameKojevAlexanderParticle = new SourceTextParticle
            {
                Content = @"Кожев, Александр",
                Order = ++pOrder
            };

            var kojevAlexanderParticle = new SourceTextParticle
            {
                Content = @"(наст. фамилия – Кожевников, 1901-1968) – французский философ-неогегельянец. Родился в Москве, эмигрировал в 1920г., образование получил в Берлине. Учился у К. Ясперса. С 1928 г. – во Франции. Стремился интерпретировать Гегеля с использованием идей марксизма и экзистенциализма. Трактовку конца истории по Гегелю Кожев соединяет с марксизмом: если у Гегеля конец истории приносит Наполеон, уничтожающий социальные институты, то Кожев пишет: «…Конец истории – это не Наполеон, это Сталин…»",
                Order = ++pOrder
            };

            var nameGessGermanParticle = new SourceTextParticle
            {
                Content = @"Гессе, Герман",
                Order = ++pOrder
            };

            var gessGermanParticle = new SourceTextParticle
            {
                Content = @"(1877-1962) – немецкий писатель, нобелевский лауреат. В годы Первой мировой войны занимал антивоенные позиции. Решительно отмежевался от фашизма. Автор романов «Петер Каменцинд», «Демиан», «Степной волк», «Игра в бисер», стихотворных циклов, рассказов, критических эссе, публицистических статей.",
                Order = ++pOrder
            };

            var nameCityUponHillParticle = new SourceTextParticle
            {
                Content = @"«Град на холме»",
                Order = ++pOrder
            };

            var cityUponHillParticle = new SourceTextParticle
            {
                Content = @" (City upon a Hill) – символический образ благого мира, описанный проповедником Джоном Уинтропом в 1630 году с использованием стихов Евангелия от Матфея. Паства, к которой обращался с проповедью Уинтроп, должна была создать этот новый благой мир, возвышающийся над миром старым и греховным. Образ-утопия «Град на холме» стал очень популярным в Америке и до сих пор воспринимается многими американцами и как миссия, и как символ США ",
                Order = ++pOrder
            };

            #endregion
            #region Define tags
            #endregion
            
            #region Define blocks
            var fukuyamaBlock = new Block
            {
                Caption = "Фукуяма, Френсис",
                Particles =
                    new List<Particle> { new QuoteSourceParticle { SourceTextParticle = fukuyamaParticle, Order = 0 } },
            };
            
            var kojevBlock = new Block
            {
                Caption = "Кожев, Александр",
                Particles =
                    new List<Particle> { new QuoteSourceParticle { SourceTextParticle = kojevAlexanderParticle, Order = 0 } },
            };

            var hegelBlock = new Block
            {
                Caption = "Гегель, Георг"
            };

            var gesseBlock = new Block
            {
                Caption = "Гессе, Герман",
                Particles =
                    new List<Particle> { new QuoteSourceParticle { SourceTextParticle = gessGermanParticle, Order = 0 } }
            };

            var cityUpinHillBlock = new Block
            {
                Caption = "«Град на холме»",
                Particles =
                    new List<Particle> { new QuoteSourceParticle { SourceTextParticle = cityUponHillParticle, Order = 0 } }
            };

            var gameBlock = new Block {Caption = "Игра"};
            var elitsBlock = new Block { Caption = "Элиты" };
            var ideologicalWarBlock = new Block { Caption = "Идеологическая война" };
            var ideologicalWarBlock = new Block { Caption = "«Игра в бисер»" };
            var ideologicalWarBlock = new Block { Caption = "«Исторический огонь" };
            var ideologicalWarBlock = new Block { Caption = "Европа" };
            var ideologicalWarBlock = new Block { Caption = "США" };
            var ideologicalWarBlock = new Block { Caption = "Ислам" };
            var ideologicalWarBlock = new Block { Caption = "Латинская Америка" };
            var ideologicalWarBlock = new Block { Caption = "Россия" };



            #endregion

            #region Define relations

            #endregion

            #region Define references

            #endregion
        }
        private static void SeedChapter3(MemOrgContext context)
        {
            #region Define particles
            #endregion
            #region Define tags
            #endregion
            #region Define blocks
            #endregion
            #region Define relations
            #endregion
            #region Define references
            #endregion
        }
        private static void SeedChapter4(MemOrgContext context)
        {
            #region Define particles
            #endregion
            #region Define tags
            #endregion
            #region Define blocks
            #endregion
            #region Define relations
            #endregion
            #region Define references
            #endregion
        }
        private static void SeedChapter5(MemOrgContext context)
        {
            #region Define particles
            #endregion
            #region Define tags
            #endregion
            #region Define blocks
            #endregion
            #region Define relations
            #endregion
            #region Define references
            #endregion
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