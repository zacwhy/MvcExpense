using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Zac.MvcWhitespaceFilterAttribute
{
    class WhitespaceFilterStream : Stream
    {
        private Stream Base;

        public WhitespaceFilterStream( Stream ResponseStream )
        {
            if ( ResponseStream == null )
            {
                throw new ArgumentNullException( "ResponseStream" );
            }

            this.Base = ResponseStream;
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            string html = Encoding.UTF8.GetString( buffer, offset, count );

            //Thanks to Qtax
            //http://stackoverflow.com/questions/8762993/remove-white-space-from-entire-html-but-inside-pre-with-regular-expressions
            //var reg = new Regex( @"(?<=\s)\s+(?![^<>]*</pre>)" ); // todo put this back - to ignore inside <pre> tags
            //html = reg.Replace( html, string.Empty );

            // by zy
            var reg = new Regex( @"(?<=\s)\s+(?![^<>]*</script>)" );
            html = reg.Replace( html, string.Empty );

            // by zy - remove line feed
            //html = Regex.Replace( html, @"\r\n?|\n", string.Empty ); // todo put this back

            buffer = Encoding.UTF8.GetBytes( html );
            this.Base.Write( buffer, 0, buffer.Length );
        }

        public override int Read( byte[] buffer, int offset, int count )
        {
            throw new NotSupportedException();
        }

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override void Flush()
        {
            Base.Flush();
        }

        public override long Seek( long offset, SeekOrigin origin )
        {
            throw new NotSupportedException();
        }

        public override void SetLength( long value )
        {
            throw new NotSupportedException();
        }

    }
}
