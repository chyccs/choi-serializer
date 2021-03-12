using System;
using System.Linq;
using System.Collections.Generic;

namespace ChoiSerializer.Support
{
    public delegate byte[] EscapeDelegate(byte input);

    public class ByteBuffer : List<byte>, IDisposable
    {
        public void Dispose()
        {
            Clear();
            GC.SuppressFinalize(this);
        }

        public int PositionToWrite { get; private set; } = 0;

        public int PositionToRead { get; private set; } = 0;

        private EscapeDelegate EscapeDelegate { get; set; }

        public void Rewind()
        {
            PositionToWrite = 0;
            PositionToRead = 0;
        }

        public int Size
        {
            get { return this.Count; }
        }

        public ByteBuffer(byte[] data = null, EscapeDelegate escape = null )
        {
            EscapeDelegate = escape;
            if (data != null)
                AddRange(data.ToList());
        }

        public new void Clear()
        {
            PositionToWrite = 0;
            PositionToRead = 0;
            base.Clear();
        }

        public ByteBuffer Put( byte[] inputs )
        {
            foreach ( byte b in inputs )
            {
                Put( b );
            }

            return this;
        }

        public ByteBuffer Put( byte[] inputs, int length )
        {
            int alength = inputs.Length < length ? inputs.Length : length;
            
            byte[] result = new byte[length];

            Array.Clear( result, 0, length );
            Array.Copy( inputs, 0, result, 0, alength );

            return Put( result );
        }

        public ByteBuffer Put( byte input, bool escapeIfExist = true )
        {
            if (EscapeDelegate != null && escapeIfExist )
            {
                byte[] escDatas = EscapeDelegate(input);

                foreach ( byte item in escDatas )
                {
                    PutByte( item );
                }
            }
            else
            {
                PutByte( input );
            }

            return this;
        }

        private ByteBuffer PutByte( byte input )
        {
            Add(input);
            PositionToWrite++;
            return this;
        }

        public ByteBuffer PutNull( int length )
        {
            for ( int i=0;i < length; i++ )
            {
                Put( 0x00 );
            }

            return this;
        }

        public ByteBuffer PutInt(int input)
        {
            return Put( ByteConverter.IntToByte( input ) );
        }

        public ByteBuffer PutLong( long input )
        {
            return Put( ByteConverter.LongToByte( input ) );
        }

        public ByteBuffer PutShort( short input )
        {
            return Put( ByteConverter.ShortToByte( input ) );
        }

        public ByteBuffer PutUShort(ushort input)
        {
            return Put(BitConverter.GetBytes(input));
        }

        public ByteBuffer PutUInt(uint input)
        {
            return Put(BitConverter.GetBytes(input));
        }

        public ByteBuffer PutULong(ulong input)
        {
            return Put(BitConverter.GetBytes(input));
        }

        public ByteBuffer PutSingle(Single input)
        {
            return Put(BitConverter.GetBytes(input));
        }

        #region get

        public int GetInt()
        {
            return BitConverter.ToInt32( GetBytes( 4 ), 0 );
        }

        public short GetShort()
        {
            return BitConverter.ToInt16( GetBytes( 2 ), 0 );
        }

		public ushort GetUShort()
		{
			return BitConverter.ToUInt16(GetBytes(2), 0);
		}

        public long GetLong()
        {
            return BitConverter.ToInt64( GetBytes( 8 ), 0 );
        }

        public byte[] GetBytes()
        {
            return GetBytes( PositionToWrite - PositionToRead );
        }

        public byte[] GetBytes(int size)
        {
            PositionToRead += size;
            return this.Skip(PositionToRead - size).Take(size).ToArray();
        }

        public byte GetByte()
        {
            return GetBytes(1)[0];
        }

        public int GetByteToInt()
        {
            return (int)( GetByte() & 0xFF );
        }

        public string GetString( int length )
        {
            return System.Text.Encoding.UTF8.GetString( GetBytes( length ) ).Trim('\0');
        }

        public byte GetChecksum(int length)
        {
            return (byte)this.Skip(PositionToRead).Take(length).Aggregate(0, (checkSum, item) => { checkSum += (int)(item & 0xFF); return checkSum; });
        }

        #endregion
    }
}
