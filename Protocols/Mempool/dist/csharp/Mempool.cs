// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Mempool.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbr = global::Google.Protobuf.Reflection;

namespace ADL.Protocol.Mempool.dist.csharp {

  /// <summary>Holder for reflection information generated from Mempool.proto</summary>
  public static partial class MempoolReflection {

    #region Descriptor
    /// <summary>File descriptor for Mempool.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static MempoolReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg1NZW1wb29sLnByb3RvEhVBREwuUHJvdG9jb2xzLk1lbXBvb2wingIKAlR4",
            "EhUKDWFkZHJlc3NTb3VyY2UYASABKAkSEwoLYWRkcmVzc0Rlc3QYAiABKAkS",
            "EQoJc2lnbmF0dXJlGAMgASgJEg4KBmFtb3VudBgEIAEoBxILCgNmZWUYBSAB",
            "KAcSFAoMb3V0cHV0QW1vdW50GAYgASgHEhMKC2lucHV0QWN0aW9uGAcgASgJ",
            "EhQKDHVubG9ja1NjcmlwdBgIIAEoCRIYChB1bmxvY2tpbmdQcm9ncmFtGAkg",
            "ASgJEjQKB3VwZGF0ZWQYCiABKAsyIy5BREwuUHJvdG9jb2xzLk1lbXBvb2wu",
            "VHguVGltZXN0YW1wGisKCVRpbWVzdGFtcBIPCgdzZWNvbmRzGAEgASgDEg0K",
            "BW5hbm9zGAIgASgFIh8KA0tleRIYChBoYXNoZWRfc2lnbmF0dXJlGAEgASgJ",
            "YgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::ADL.Protocol.Mempool.dist.csharp.Tx), global::ADL.Protocol.Mempool.dist.csharp.Tx.Parser, new[]{ "AddressSource", "AddressDest", "Signature", "Amount", "Fee", "OutputAmount", "InputAction", "UnlockScript", "UnlockingProgram", "Updated" }, null, null, new pbr::GeneratedClrTypeInfo[] { new pbr::GeneratedClrTypeInfo(typeof(global::ADL.Protocol.Mempool.dist.csharp.Tx.Types.Timestamp), global::ADL.Protocol.Mempool.dist.csharp.Tx.Types.Timestamp.Parser, new[]{ "Seconds", "Nanos" }, null, null, null)}),
            new pbr::GeneratedClrTypeInfo(typeof(global::ADL.Protocol.Mempool.dist.csharp.Key), global::ADL.Protocol.Mempool.dist.csharp.Key.Parser, new[]{ "HashedSignature" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Tx : pb::IMessage<Tx> {
    private static readonly pb::MessageParser<Tx> _parser = new pb::MessageParser<Tx>(() => new Tx());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Tx> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::ADL.Protocol.Mempool.dist.csharp.MempoolReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Tx() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Tx(Tx other) : this() {
      addressSource_ = other.addressSource_;
      addressDest_ = other.addressDest_;
      signature_ = other.signature_;
      amount_ = other.amount_;
      fee_ = other.fee_;
      outputAmount_ = other.outputAmount_;
      inputAction_ = other.inputAction_;
      unlockScript_ = other.unlockScript_;
      unlockingProgram_ = other.unlockingProgram_;
      updated_ = other.updated_ != null ? other.updated_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Tx Clone() {
      return new Tx(this);
    }

    /// <summary>Field number for the "addressSource" field.</summary>
    public const int AddressSourceFieldNumber = 1;
    private string addressSource_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string AddressSource {
      get { return addressSource_; }
      set {
        addressSource_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "addressDest" field.</summary>
    public const int AddressDestFieldNumber = 2;
    private string addressDest_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string AddressDest {
      get { return addressDest_; }
      set {
        addressDest_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "signature" field.</summary>
    public const int SignatureFieldNumber = 3;
    private string signature_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Signature {
      get { return signature_; }
      set {
        signature_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "amount" field.</summary>
    public const int AmountFieldNumber = 4;
    private uint amount_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint Amount {
      get { return amount_; }
      set {
        amount_ = value;
      }
    }

    /// <summary>Field number for the "fee" field.</summary>
    public const int FeeFieldNumber = 5;
    private uint fee_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint Fee {
      get { return fee_; }
      set {
        fee_ = value;
      }
    }

    /// <summary>Field number for the "outputAmount" field.</summary>
    public const int OutputAmountFieldNumber = 6;
    private uint outputAmount_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint OutputAmount {
      get { return outputAmount_; }
      set {
        outputAmount_ = value;
      }
    }

    /// <summary>Field number for the "inputAction" field.</summary>
    public const int InputActionFieldNumber = 7;
    private string inputAction_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string InputAction {
      get { return inputAction_; }
      set {
        inputAction_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "unlockScript" field.</summary>
    public const int UnlockScriptFieldNumber = 8;
    private string unlockScript_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UnlockScript {
      get { return unlockScript_; }
      set {
        unlockScript_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "unlockingProgram" field.</summary>
    public const int UnlockingProgramFieldNumber = 9;
    private string unlockingProgram_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UnlockingProgram {
      get { return unlockingProgram_; }
      set {
        unlockingProgram_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "updated" field.</summary>
    public const int UpdatedFieldNumber = 10;
    private global::ADL.Protocol.Mempool.dist.csharp.Tx.Types.Timestamp updated_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::ADL.Protocol.Mempool.dist.csharp.Tx.Types.Timestamp Updated {
      get { return updated_; }
      set {
        updated_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Tx);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Tx other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (AddressSource != other.AddressSource) return false;
      if (AddressDest != other.AddressDest) return false;
      if (Signature != other.Signature) return false;
      if (Amount != other.Amount) return false;
      if (Fee != other.Fee) return false;
      if (OutputAmount != other.OutputAmount) return false;
      if (InputAction != other.InputAction) return false;
      if (UnlockScript != other.UnlockScript) return false;
      if (UnlockingProgram != other.UnlockingProgram) return false;
      if (!object.Equals(Updated, other.Updated)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (AddressSource.Length != 0) hash ^= AddressSource.GetHashCode();
      if (AddressDest.Length != 0) hash ^= AddressDest.GetHashCode();
      if (Signature.Length != 0) hash ^= Signature.GetHashCode();
      if (Amount != 0) hash ^= Amount.GetHashCode();
      if (Fee != 0) hash ^= Fee.GetHashCode();
      if (OutputAmount != 0) hash ^= OutputAmount.GetHashCode();
      if (InputAction.Length != 0) hash ^= InputAction.GetHashCode();
      if (UnlockScript.Length != 0) hash ^= UnlockScript.GetHashCode();
      if (UnlockingProgram.Length != 0) hash ^= UnlockingProgram.GetHashCode();
      if (updated_ != null) hash ^= Updated.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (AddressSource.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(AddressSource);
      }
      if (AddressDest.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(AddressDest);
      }
      if (Signature.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Signature);
      }
      if (Amount != 0) {
        output.WriteRawTag(37);
        output.WriteFixed32(Amount);
      }
      if (Fee != 0) {
        output.WriteRawTag(45);
        output.WriteFixed32(Fee);
      }
      if (OutputAmount != 0) {
        output.WriteRawTag(53);
        output.WriteFixed32(OutputAmount);
      }
      if (InputAction.Length != 0) {
        output.WriteRawTag(58);
        output.WriteString(InputAction);
      }
      if (UnlockScript.Length != 0) {
        output.WriteRawTag(66);
        output.WriteString(UnlockScript);
      }
      if (UnlockingProgram.Length != 0) {
        output.WriteRawTag(74);
        output.WriteString(UnlockingProgram);
      }
      if (updated_ != null) {
        output.WriteRawTag(82);
        output.WriteMessage(Updated);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (AddressSource.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AddressSource);
      }
      if (AddressDest.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AddressDest);
      }
      if (Signature.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Signature);
      }
      if (Amount != 0) {
        size += 1 + 4;
      }
      if (Fee != 0) {
        size += 1 + 4;
      }
      if (OutputAmount != 0) {
        size += 1 + 4;
      }
      if (InputAction.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(InputAction);
      }
      if (UnlockScript.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UnlockScript);
      }
      if (UnlockingProgram.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UnlockingProgram);
      }
      if (updated_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Updated);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Tx other) {
      if (other == null) {
        return;
      }
      if (other.AddressSource.Length != 0) {
        AddressSource = other.AddressSource;
      }
      if (other.AddressDest.Length != 0) {
        AddressDest = other.AddressDest;
      }
      if (other.Signature.Length != 0) {
        Signature = other.Signature;
      }
      if (other.Amount != 0) {
        Amount = other.Amount;
      }
      if (other.Fee != 0) {
        Fee = other.Fee;
      }
      if (other.OutputAmount != 0) {
        OutputAmount = other.OutputAmount;
      }
      if (other.InputAction.Length != 0) {
        InputAction = other.InputAction;
      }
      if (other.UnlockScript.Length != 0) {
        UnlockScript = other.UnlockScript;
      }
      if (other.UnlockingProgram.Length != 0) {
        UnlockingProgram = other.UnlockingProgram;
      }
      if (other.updated_ != null) {
        if (updated_ == null) {
          updated_ = new global::ADL.Protocol.Mempool.dist.csharp.Tx.Types.Timestamp();
        }
        Updated.MergeFrom(other.Updated);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            AddressSource = input.ReadString();
            break;
          }
          case 18: {
            AddressDest = input.ReadString();
            break;
          }
          case 26: {
            Signature = input.ReadString();
            break;
          }
          case 37: {
            Amount = input.ReadFixed32();
            break;
          }
          case 45: {
            Fee = input.ReadFixed32();
            break;
          }
          case 53: {
            OutputAmount = input.ReadFixed32();
            break;
          }
          case 58: {
            InputAction = input.ReadString();
            break;
          }
          case 66: {
            UnlockScript = input.ReadString();
            break;
          }
          case 74: {
            UnlockingProgram = input.ReadString();
            break;
          }
          case 82: {
            if (updated_ == null) {
              updated_ = new global::ADL.Protocol.Mempool.dist.csharp.Tx.Types.Timestamp();
            }
            input.ReadMessage(updated_);
            break;
          }
        }
      }
    }

    #region Nested types
    /// <summary>Container for nested types declared in the Tx message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      public sealed partial class Timestamp : pb::IMessage<Timestamp> {
        private static readonly pb::MessageParser<Timestamp> _parser = new pb::MessageParser<Timestamp>(() => new Timestamp());
        private pb::UnknownFieldSet _unknownFields;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pb::MessageParser<Timestamp> Parser { get { return _parser; } }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pbr::MessageDescriptor Descriptor {
          get { return global::ADL.Protocol.Mempool.dist.csharp.Tx.Descriptor.NestedTypes[0]; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        pbr::MessageDescriptor pb::IMessage.Descriptor {
          get { return Descriptor; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public Timestamp() {
          OnConstruction();
        }

        partial void OnConstruction();

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public Timestamp(Timestamp other) : this() {
          seconds_ = other.seconds_;
          nanos_ = other.nanos_;
          _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public Timestamp Clone() {
          return new Timestamp(this);
        }

        /// <summary>Field number for the "seconds" field.</summary>
        public const int SecondsFieldNumber = 1;
        private long seconds_;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public long Seconds {
          get { return seconds_; }
          set {
            seconds_ = value;
          }
        }

        /// <summary>Field number for the "nanos" field.</summary>
        public const int NanosFieldNumber = 2;
        private int nanos_;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public int Nanos {
          get { return nanos_; }
          set {
            nanos_ = value;
          }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override bool Equals(object other) {
          return Equals(other as Timestamp);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public bool Equals(Timestamp other) {
          if (ReferenceEquals(other, null)) {
            return false;
          }
          if (ReferenceEquals(other, this)) {
            return true;
          }
          if (Seconds != other.Seconds) return false;
          if (Nanos != other.Nanos) return false;
          return Equals(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override int GetHashCode() {
          int hash = 1;
          if (Seconds != 0L) hash ^= Seconds.GetHashCode();
          if (Nanos != 0) hash ^= Nanos.GetHashCode();
          if (_unknownFields != null) {
            hash ^= _unknownFields.GetHashCode();
          }
          return hash;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override string ToString() {
          return pb::JsonFormatter.ToDiagnosticString(this);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void WriteTo(pb::CodedOutputStream output) {
          if (Seconds != 0L) {
            output.WriteRawTag(8);
            output.WriteInt64(Seconds);
          }
          if (Nanos != 0) {
            output.WriteRawTag(16);
            output.WriteInt32(Nanos);
          }
          if (_unknownFields != null) {
            _unknownFields.WriteTo(output);
          }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public int CalculateSize() {
          int size = 0;
          if (Seconds != 0L) {
            size += 1 + pb::CodedOutputStream.ComputeInt64Size(Seconds);
          }
          if (Nanos != 0) {
            size += 1 + pb::CodedOutputStream.ComputeInt32Size(Nanos);
          }
          if (_unknownFields != null) {
            size += _unknownFields.CalculateSize();
          }
          return size;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(Timestamp other) {
          if (other == null) {
            return;
          }
          if (other.Seconds != 0L) {
            Seconds = other.Seconds;
          }
          if (other.Nanos != 0) {
            Nanos = other.Nanos;
          }
          _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(pb::CodedInputStream input) {
          uint tag;
          while ((tag = input.ReadTag()) != 0) {
            switch(tag) {
              default:
                _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
                break;
              case 8: {
                Seconds = input.ReadInt64();
                break;
              }
              case 16: {
                Nanos = input.ReadInt32();
                break;
              }
            }
          }
        }

      }

    }
    #endregion

  }

  public sealed partial class Key : pb::IMessage<Key> {
    private static readonly pb::MessageParser<Key> _parser = new pb::MessageParser<Key>(() => new Key());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Key> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::ADL.Protocol.Mempool.dist.csharp.MempoolReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Key() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Key(Key other) : this() {
      hashedSignature_ = other.hashedSignature_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Key Clone() {
      return new Key(this);
    }

    /// <summary>Field number for the "hashed_signature" field.</summary>
    public const int HashedSignatureFieldNumber = 1;
    private string hashedSignature_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string HashedSignature {
      get { return hashedSignature_; }
      set {
        hashedSignature_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Key);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Key other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (HashedSignature != other.HashedSignature) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (HashedSignature.Length != 0) hash ^= HashedSignature.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (HashedSignature.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(HashedSignature);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (HashedSignature.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(HashedSignature);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Key other) {
      if (other == null) {
        return;
      }
      if (other.HashedSignature.Length != 0) {
        HashedSignature = other.HashedSignature;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            HashedSignature = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
