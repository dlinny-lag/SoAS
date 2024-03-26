using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SceneModel;
using SceneModel.ContactAreas;
using SceneServices.TagCategories;

namespace Services.UnitTests
{
    [TestClass]
    public class ContactResolverTest
    {

        [DataTestMethod]
        [DataRow("TongueToEither", typeof(Tongue), typeof(Either))]
        [DataRow("StrapOnToVagina", typeof(Strapon), typeof(Vagina))]
        [DataRow("TentacleToButt", typeof(SingleTentacle), typeof(Butt))]
        [DataRow("StickToAnus", typeof(Stick), typeof(Anus))]
        [DataRow("PenisToVagina", typeof(Penis), typeof(Vagina))]
        [DataRow("SpankToButt", typeof(Hand), typeof(Butt))]
        public void ThemeTagOK(string tag, Type from, Type to)
        {
            Assert.IsTrue(tag.TryParseFromThemeTag<ActorsContact, ParticipantContactDetails>(null, out var contact));
            Assert.AreEqual(from, contact.From.Contact.GetType());
            Assert.AreEqual(to, contact.To.Contact.GetType());
            Assert.IsFalse(tag.TryParseFromAAFInformerTag<ActorsContact, ParticipantContactDetails>(null, out _));
        }


        [TestMethod]
        public void InformerTagOK()
        {
            string tag = "HandToEither:3-2";
            Assert.IsTrue(tag.TryParseFromAAFInformerTag<ActorsContact, ParticipantContactDetails>(null, out var contact));
            Assert.AreEqual(Hand.Any, contact.From.Contact);
            Assert.AreEqual(Either.Any, contact.To.Contact);
            Assert.AreEqual(3, contact.From.ParticipantIndex);
            Assert.AreEqual(2, contact.To.ParticipantIndex);
            
            Assert.IsFalse(tag.TryParseFromThemeTag<ActorsContact, ParticipantContactDetails>(null, out _));
        }

        [TestMethod]
        public void NonContactTag()
        {
            Assert.IsFalse("OneToOther".TryParseFromThemeTag<ActorsContact, ParticipantContactDetails>(null, out _));
            Assert.IsFalse("OneToOther".TryParseFromAAFInformerTag<ActorsContact, ParticipantContactDetails>(null, out _));

            Assert.IsFalse("abracadabra".TryParseFromThemeTag<ActorsContact, ParticipantContactDetails>(null, out _));
            Assert.IsFalse("abracadabra".TryParseFromAAFInformerTag<ActorsContact, ParticipantContactDetails>(null, out _));

            Assert.IsFalse("TO".TryParseFromThemeTag<ActorsContact, ParticipantContactDetails>(null, out _));
            Assert.IsFalse("TO".TryParseFromAAFInformerTag<ActorsContact, ParticipantContactDetails>(null, out _));

            Assert.IsFalse("PenisTO".TryParseFromThemeTag<ActorsContact, ParticipantContactDetails>(null, out _));
            Assert.IsFalse("PenisTO".TryParseFromAAFInformerTag<ActorsContact, ParticipantContactDetails>(null, out _));

            Assert.IsFalse("TongueToAbracadabra".TryParseFromThemeTag<ActorsContact, ParticipantContactDetails>(null, out _));
            Assert.IsFalse("TongueToAbracadabra".TryParseFromAAFInformerTag<ActorsContact, ParticipantContactDetails>(null, out _));

        }
    }
}